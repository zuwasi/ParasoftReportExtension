# Sample Report Files

This directory contains sample report files for testing the Parasoft C++TEST Report Extension.

## Files

### sample.sarif
- **Format**: SARIF 2.1.0
- **Content**: Contains 3 sample violations (1 error, 1 warning, 1 info)
- **Use**: Test SARIF parsing functionality

### sample.html
- **Format**: HTML with table-based structure
- **Content**: Same violations as SARIF file but in HTML format
- **Use**: Test HTML parsing functionality

## How to Use

1. Install the Parasoft C++TEST Report Extension
2. Open Visual Studio 2022
3. Go to **Tools** → **Load Parasoft C++TEST Report**
4. Select either `sample.sarif` or `sample.html`
5. Check the Error List to see the loaded violations

## Expected Results

When you load either file, you should see:

| File | Line | Rule ID | Severity | Message |
|------|------|---------|----------|---------|
| src/main.cpp | 42 | BD-PB-CC | Warning | Avoid using 'goto' statements |
| src/utils.cpp | 15 | CODSTA-CPP-23 | Information | Variable 'temp' should be declared in smaller scope |
| src/parser.cpp | 78 | MISRA2012-RULE-2_2 | Error | Unreachable code detected after return statement |

## Notes

- The source files referenced in these reports (`src/main.cpp`, `src/utils.cpp`, `src/parser.cpp`) don't actually exist
- Double-clicking on errors will show a "file not found" message, which is expected
- These files are for testing the parsing and Error List integration functionality
