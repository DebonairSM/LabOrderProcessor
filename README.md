# LabOrderProcessor

An Invene assessment application to redact PHI from lab orders.

## Description

LabOrderProcessor is a C# application designed to process and redact Protected Health Information (PHI) from lab orders. This project aims to ensure compliance with privacy regulations by removing sensitive information from lab order documents.

## Features

- Redacts PHI from lab orders
- Ensures compliance with privacy regulations

## Getting Started

### Prerequisites

- [.NET Core](https://dotnet.microsoft.com/download/dotnet-core)

### Installation

1. Clone the repository:

   ```sh
   git clone https://github.com/DebonairSM/LabOrderProcessor.git
   ```

2. Navigate to the project directory:

   ```sh
   cd LabOrderProcessor
   ```

3. Build the project:

   ```sh
   dotnet build
   ```

### Usage

To run the application:

```sh
dotnet run
```

## LabOrderProcessor.Web

This project was generated using [Angular CLI](https://github.com/angular/angular-cli) version 19.0.0.

### Development server

To start a local development server, run:

```bash
ng serve
```

Once the server is running, open your browser and navigate to `http://localhost:4200/`. The application will automatically reload whenever you modify any of the source files.

### Code scaffolding

Angular CLI includes powerful code scaffolding tools. To generate a new component, run:

```bash
ng generate component component-name
```

For a complete list of available schematics (such as `components`, `directives`, or `pipes`), run:

```bash
ng generate --help
```

### Building

To build the project run:

```bash
ng build
```

This will compile your project and store the build artifacts in the `dist/` directory. By default, the production build optimizes your application for performance and speed.

### Running unit tests

To execute unit tests with the [Karma](https://karma-runner.github.io) test runner, use the following command:

```bash
ng test
```

### Running end-to-end tests

For end-to-end (e2e) testing, run:

```bash
ng e2e
```

Angular CLI does not come with an end-to-end testing framework by default. You can choose one that suits your needs.

### Additional Resources

For more information on using the Angular CLI, including detailed command references, visit the [Angular CLI Overview and Command Reference](https://angular.dev/tools/cli) page.

## Approach to Identifying and Redacting PHI

The process involves using regular expressions to match and redact specific patterns that correspond to PHI. The `PhiDetectionService` class handles the redaction by applying predefined patterns to the content of lab orders. The redaction patterns include:

- Patient Name
- Date of Birth
- Social Security Number
- Address
- Phone Number
- Email
- Medical Record Number

Example of redaction pattern:

```csharp
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
```

## Assumptions, Limitations, and Future Improvements

### Assumptions

- The input lab orders follow a consistent format suitable for regex-based redaction.
- All PHI elements are covered by the predefined redaction patterns.

### Limitations

- The current redaction method may not handle all possible variations of PHI formats.
- The application assumes text-based input and may not work with other formats (e.g., scanned images).

### Areas for Future Improvement

- Enhance the regex patterns to cover more variations of PHI.
- Implement machine learning techniques to identify PHI more accurately.
- Add support for handling different input formats, such as PDFs and images.
- Improve the logging and error handling mechanisms.

## Contributors

- [DebonairSM](https://github.com/DebonairSM)

## License

This project is licensed under the Apache License 2.0 - see the [LICENSE](LICENSE) file for details.
```
