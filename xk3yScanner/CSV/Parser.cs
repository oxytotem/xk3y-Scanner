using System;
using System.Collections.Generic;
using System.Text;

namespace QiHe.CodeLib.Csv
{
	public partial class Parser
	{
		public CsvData ParseCsvData(ParserInput<char> input, out bool success)
		{
			this.SetInput(input);
			CsvData csvData = ParseCsvData(out success);
			return csvData;
		}

		private CsvData ParseCsvData(out bool success)
		{
			int errorCount = Errors.Count;
			CsvData csvData = new CsvData();
			int start_position = position;

			while (true)
			{
				CsvRecord csvRecord_1 = ParseCsvRecord(out success);
				if (success) { csvData.Records.Add(csvRecord_1); }
				else { break; }
			}
			success = true;

			ParseEndOfFile(out success);
			if (!success)
			{
				Error("Failed to parse EndOfFile of CsvData.");
				position = start_position;
			}

			if (success) { ClearError(errorCount); }
			return csvData;
		}

		private CsvRecord ParseCsvRecord(out bool success)
		{
			int errorCount = Errors.Count;
			CsvRecord csvRecord = new CsvRecord();
			int start_position = position;

			int not_start_position1 = position;
			ParseEndOfFile(out success);
			position = not_start_position1;
			success = !success;
			if (!success)
			{
				Error("Failed to parse !(EndOfFile) of CsvRecord.");
				position = start_position;
				return csvRecord;
			}

			while (true)
			{
				int seq_start_position2 = position;
				string str_1 = ParseField(out success);
				if (success) { csvRecord.Fields.Add(str_1); }
				else
				{
					Error("Failed to parse Field of CsvRecord.");
					break;
				}

				while (true)
				{
					while (true)
					{
						int seq_start_position3 = position;
						ParseSeparator(out success);
						if (!success)
						{
							Error("Failed to parse Separator of CsvRecord.");
							break;
						}

						str_1 = ParseField(out success);
						if (success) { csvRecord.Fields.Add(str_1); }
						else
						{
							Error("Failed to parse Field of CsvRecord.");
							position = seq_start_position3;
						}
						break;
					}
					if (!success) { break; }
				}
				success = true;
				break;
			}
			if (!success)
			{
				Error("Failed to parse Fields of CsvRecord.");
				position = start_position;
				return csvRecord;
			}

			ErrorStatck.Push(errorCount); errorCount = Errors.Count;
			while (true)
			{
				ParseEnfOfLine(out success);
				if (success) { ClearError(errorCount); break; }

				ParseEndOfFile(out success);
				if (success) { ClearError(errorCount); break; }

				break;
			}
			errorCount = ErrorStatck.Pop();
			if (!success)
			{
				Error("Failed to parse (EnfOfLine / EndOfFile) of CsvRecord.");
				position = start_position;
			}

			if (success) { ClearError(errorCount); }
			return csvRecord;
		}

		private string ParseField(out bool success)
		{
			int errorCount = Errors.Count;
			string str = null;
			int start_position = position;

			ParseSpacing(out success);

			ErrorStatck.Push(errorCount); errorCount = Errors.Count;
			while (true)
			{
				str = ParseQuotedText(out success);
				if (success) { ClearError(errorCount); break; }

				str = ParseUnQuotedText(out success);
				if (success) { ClearError(errorCount); break; }

				break;
			}
			errorCount = ErrorStatck.Pop();
			if (!success)
			{
				Error("Failed to parse (QuotedText / UnQuotedText) of Field.");
				position = start_position;
			}

			if (success) { ClearError(errorCount); }
			return str;
		}

		private string ParseUnQuotedText(out bool success)
		{
			int errorCount = Errors.Count;
			StringBuilder text = new StringBuilder();

			while (true)
			{
				char ch_1 = MatchTerminalSet(",\"\r\n", true, out success);
				if (success) { text.Append(ch_1); }
				else { break; }
			}
			success = true;
			if(text.Length==0) return null;
			return text.ToString();
		}

		private string ParseQuotedText(out bool success)
		{
			int errorCount = Errors.Count;
			StringBuilder text = new StringBuilder();
			int start_position = position;

			MatchTerminal('"', out success);
			if (!success)
			{
				Error("Failed to parse '\\\"' of QuotedText.");
				position = start_position;
				return text.ToString();
			}

			while (true)
			{
				ErrorStatck.Push(errorCount); errorCount = Errors.Count;
				while (true)
				{
					char ch_1 = MatchTerminalSet("\"\r\n\\", true, out success);
					if (success)
					{
						ClearError(errorCount);
						text.Append(ch_1);
						break;
					}

					ch_1 = ParseEscapeSequence(out success);
					if (success)
					{
						ClearError(errorCount);
						text.Append(ch_1);
						break;
					}

					break;
				}
				errorCount = ErrorStatck.Pop();
				if (!success) { break; }
			}
			success = true;

			MatchTerminal('"', out success);
			if (!success)
			{
				Error("Failed to parse '\\\"' of QuotedText.");
				position = start_position;
			}

			if (success) { ClearError(errorCount); }
			return text.ToString();
		}

		private char ParseEscapeSequence(out bool success)
		{
			int errorCount = Errors.Count;
			char ch = default(char);

			MatchTerminalString("\\\\", out success);
			if (success) { return '\\'; }

			MatchTerminalString("\\\"", out success);
			if (success) { return '\"'; }

			MatchTerminalString("\\r", out success);
			if (success) { return '\r'; }

			MatchTerminalString("\\n", out success);
			if (success) { return '\n'; }

			MatchTerminalString("\\t", out success);
			if (success) { return '\t'; }

			return ch;
		}

		private char ParseSpace(out bool success)
		{
			int errorCount = Errors.Count;
			char ch = default(char);

			ch = MatchTerminal(' ', out success);
			if (success) { ClearError(errorCount); return ch; }

			ch = MatchTerminal('\t', out success);
			if (success) { ClearError(errorCount); return ch; }

			return ch;
		}

		private void ParseSpacing(out bool success)
		{
			int errorCount = Errors.Count;
			while (true)
			{
				ParseSpace(out success);
				if (!success) { break; }
			}
			success = true;
		}

		private void ParseSeparator(out bool success)
		{
			int errorCount = Errors.Count;
			MatchTerminal(',', out success);
			if (success) { ClearError(errorCount); }
			else { Error("Failed to parse ',' of Separator."); }
		}

		private void ParseEnfOfLine(out bool success)
		{
			int errorCount = Errors.Count;
			MatchTerminalString("\r\n", out success);
			if (success) { ClearError(errorCount); return; }

			MatchTerminal('\r', out success);
			if (success) { ClearError(errorCount); return; }

			MatchTerminal('\n', out success);
			if (success) { ClearError(errorCount); return; }

		}

		private void ParseEndOfFile(out bool success)
		{
			int errorCount = Errors.Count;
			success = !Input.HasInput(position);
			if (success) { ClearError(errorCount); }
			else { Error("Failed to parse end of EndOfFile."); }
		}

	}
}
