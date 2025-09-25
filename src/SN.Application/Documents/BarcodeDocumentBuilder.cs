using SN.Applications.Documents.Dtos;
using SN.Core.Domain;
using SN.Core.Domain.Barcodes;
using SN.Core.Domain.Documents;
using SN.Core.Domain.Epcis;
using SN.Core.Domain.ValueObjects;

namespace SN.Applications.Documents;


public class BarcodeDocumentBuilder
{
    private CoreDocumentDto _coreDocument;
    public BarcodeDocumentBuilder(CoreDocumentDto coreDocument)
    {
        _coreDocument = coreDocument;
    }

    public SNDocument GetResult()
    {
        var snDocument = new SNDocument(_coreDocument.SenderIdentifier, _coreDocument.ReceiverIdentifier, _coreDocument.Owner);
        snDocument.SetFileInfo(_coreDocument.FileName, _coreDocument.FileExtension, _coreDocument.DocumentIdentifier, _coreDocument.TransactionCode);
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
        var epcis = new EpcisNode(node.Id, node.Level, document.Id, GetParentid(node.Parentid, parents));
        parents.Add(epcis.EpcisCode, epcis.Id);
        document.AddEpcis(epcis);

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
        BPOM2DBarcode bpom = new BPOM2DBarcode(
            node.SerialCode,
            node.GtinCode,
            node.Batch != null ? new Batch(node.Batch) : null,
            node.ExpireDate != null ? DateOnly.Parse(node.ExpireDate) : null
        );

        switch (node.Level)
        {
            case AgregationLevel.Tertiary:
                var tertiary = new TertiaryBarcode(bpom, document);
                if (parentTertiary == null && parentSecondary == null)
                    document.AddBarcode(tertiary);
                foreach (var child in node.Children)
                {
                    BuildNode(child, document, tertiary, null);
                }
                break;

            case AgregationLevel.Secondary:
                var secondary = new SecondaryBarcode(bpom, parentTertiary, document);
                parentTertiary?.AddSecondary(secondary);
                if (parentTertiary == null && parentSecondary == null)
                    document.AddBarcode(secondary);
                foreach (var child in node.Children)
                {
                    BuildNode(child, document, null, secondary);
                }
                break;

            case AgregationLevel.Primary:
                var primary = new PrimaryBarcode(bpom, document, parentSecondary);
                parentSecondary?.AddPrimary(primary);
                if (parentSecondary == null)
                    document.AddBarcode(primary);
                break;
        }
    }
}