using System.Text.RegularExpressions;

namespace LabOrderProcessor.API.Services;

public class PhiDetectionService
{
    public string RedactPhi(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return content;

        var redactedContent = content;

        var redactionPatterns = new Dictionary<string, string>
        {
            { @"^Patient Name:\s*[A-Z][a-z]+\s+[A-Z][a-z]+", "Patient Name: [REDACTED]" },
            { @"^Date of Birth:\s*(?:0[1-9]|1[0-2])/([0-2][1-9]|[12][0-9]|3[01])/\d{4}", "Date of Birth: [REDACTED]" },
            { @"^Social Security Number:\s*\d{3}-\d{2}-\d{4}", "Social Security Number: [REDACTED]" },
            { @"^Address:\s*\d{1,6}[ \t]+[A-Za-z0-9 ,]+", "Address: [REDACTED]" },
            { @"^Phone Number:\s*\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}", "Phone Number: [REDACTED]" },
            { @"^Email:\s*[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}", "Email: [REDACTED]" },
            { @"^Medical Record Number:\s*MRN-\d{7}", "Medical Record Number: [REDACTED]" }
        };

        foreach (var pattern in redactionPatterns)
        {
            redactedContent = Regex.Replace(
                redactedContent,
                pattern.Key,
                pattern.Value,
                RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        return redactedContent;
    }

    public Dictionary<string, List<string>> IdentifyPhiElements(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return new Dictionary<string, List<string>>();

        var phiElements = new Dictionary<string, List<string>>();

        return phiElements;
    }
}
