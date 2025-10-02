using SN.Applications.Documents.Dtos;
using SN.Core.Domain;
using SN.Core.Domain.Barcodes;
using SN.Core.Domain.Documents;
using SN.Core.Domain.SerialNode;
using SN.Core.Domain.ValueObjects;
using SN.Core.Factories;

namespace SN.Applications.Documents;


public class BarcodeDocumentBuilder
{
    private CoreDocumentDto _coreDocument;
    private IBPOM2DBarcodeFactory _bPOM2DBarcodeFactory;
    public BarcodeDocumentBuilder(CoreDocumentDto coreDocument, IBPOM2DBarcodeFactory bPOM2DBarcodeFactory)
    {
        _coreDocument = coreDocument;
        _bPOM2DBarcodeFactory = bPOM2DBarcodeFactory;
    }

    public SNDocument GetResult()
    {
        var snDocument = new SNDocument(_coreDocument.SenderIdentifier, _coreDocument.ReceiverIdentifier, _coreDocument.DeliveryNumber, _coreDocument.Producer);
        snDocument.SetFileInfo(_coreDocument.FilePath,_coreDocument.DocumentType, _coreDocument.DocumentIdentifier, _coreDocument.DoucmentCreationTime);
        BuildEpcis(snDocument);
        BuildBarcodeAggregation(snDocument);
        return snDocument;
    }

    private void BuildEpcis(SNDocument document)
    {
        var parents = new Dictionary<string, Guid>();
        foreach (var ag in _coreDocument.Aggregations)
        {
            ProcessAggregation(ag, document, parents);
        }
    }

    private void ProcessAggregation(AggregationNode node, SNDocument document, Dictionary<string, Guid> parents)
    {
        var serialNode = new SerializedNode(node.Id, node.Level, document.Id, GetParentid(node.Parentid, parents));
        parents.Add(serialNode.SerializedCode, serialNode.Id);
        document.AddSerializedNode(serialNode);

        foreach (var child in node.Children)
        {
            ProcessAggregation(child, document, parents);
        }
    }

    private void BuildBarcodeAggregation(SNDocument document)
    {
        foreach (var node in _coreDocument.Aggregations)
        {
            BuildNode(node, document, null, null);
        }
    }

    private Guid? GetParentid(string? parentCode, Dictionary<string, Guid> parents)
    {
        if (parentCode == null) return null;
        return parents.ContainsKey(parentCode) ? parents[parentCode] : null;
    }

    private void BuildNode(AggregationNode node, SNDocument document, TertiaryBarcode? parentTertiary, SecondaryBarcode? parentSecondary)
    {
        IBPOM2DBarcode bpom = _bPOM2DBarcodeFactory.Create(
            node.SerialCode,
            node.Level,
            node.GtinCode,
            node.Batch,
            node.ExpireDate,
            node.ManufactoringDate
        );

        switch (node.Level)
        {
            case AgregationLevel.Tertiary:
                var tertiary = new TertiaryBarcode(bpom, document);
                document.AddBarcode(tertiary); // Always add
                foreach (var child in node.Children)
                {
                    BuildNode(child, document, tertiary, null);
                }
                break;

            case AgregationLevel.Secondary:
                var secondary = new SecondaryBarcode(bpom, parentTertiary, document);
                parentTertiary?.AddSecondary(secondary);
                document.AddBarcode(secondary); // Always add
                foreach (var child in node.Children)
                {
                    BuildNode(child, document, null, secondary);
                }
                break;

            case AgregationLevel.Primary:
                var primary = new PrimaryBarcode(bpom, document, parentSecondary);
                parentSecondary?.AddPrimary(primary);
                document.AddBarcode(primary); // Always add
                break;
        }
    }
}