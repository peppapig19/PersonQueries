using System;
using System.Data;
using System.Text;

namespace PersonQueries
{
	internal class RandomReader : IDataReader
	{
		int cur = 0;
		string[] row;

		readonly Random ran;
		readonly bool isPredefined;
		const string lower = "abcdefghijklmnopqrstuvwxyz";
		const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		const int alphabetLength = 26;

		public int FieldCount { get; set; }

		public RandomReader(Random ran, int fieldCount, bool isPredefined = false)
		{
			FieldCount = fieldCount;
			this.ran = ran;
			this.isPredefined = isPredefined;
		}

		public bool Read()
		{
			if (cur >= FieldCount) return false;

			row = new string[3];
			row[1] = GenerateDOB();

			if (isPredefined)
			{
				row[0] = GenerateFullName('F');
				row[2] = "m";
			}
			else
			{
				row[0] = GenerateFullName();
				row[2] = GenerateSex();
			}

			cur++;
			return true;
		}

		public object GetValue(int i) => row[i];

		public string GenerateFullName()
		{
			StringBuilder fullName = new StringBuilder();

			for (int i = 1; i <= 3; i++)
			{
				fullName.Append(upper[ran.Next(alphabetLength)]);
				fullName.Append(lower[ran.Next(alphabetLength)]);
				if (i != 3) fullName.Append(' ');
			}

			return fullName.ToString();
		}

		public string GenerateFullName(char startsWith)
		{
			StringBuilder fullName = new StringBuilder();

			for (int i = 1; i <= 3; i++)
			{
				fullName.Append(startsWith);
				fullName.Append(lower[ran.Next(alphabetLength)]);
				if (i != 3) fullName.Append(' ');
			}

			return fullName.ToString();
		}

		public string GenerateDOB()
		{
			return new DateTime(ran.Next(1950, 2023), ran.Next(1, 13), ran.Next(1, 29)).ToString("dd.MM.yyyy");
		}

		public string GenerateSex()
		{
			return ran.Next(0, 2) == 0 ? "m" : "f";
		}

		public object this[int i] => throw new NotImplementedException();

		public object this[string name] => throw new NotImplementedException();

		public int Depth => throw new NotImplementedException();

		public bool IsClosed => throw new NotImplementedException();

		public int RecordsAffected => throw new NotImplementedException();

		public void Close()
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public bool GetBoolean(int i)
		{
			throw new NotImplementedException();
		}

		public byte GetByte(int i)
		{
			throw new NotImplementedException();
		}

		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			throw new NotImplementedException();
		}

		public char GetChar(int i)
		{
			throw new NotImplementedException();
		}

		public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			throw new NotImplementedException();
		}

		public IDataReader GetData(int i)
		{
			throw new NotImplementedException();
		}

		public string GetDataTypeName(int i)
		{
			throw new NotImplementedException();
		}

		public DateTime GetDateTime(int i)
		{
			throw new NotImplementedException();
		}

		public decimal GetDecimal(int i)
		{
			throw new NotImplementedException();
		}

		public double GetDouble(int i)
		{
			throw new NotImplementedException();
		}

		public Type GetFieldType(int i)
		{
			throw new NotImplementedException();
		}

		public float GetFloat(int i)
		{
			throw new NotImplementedException();
		}

		public Guid GetGuid(int i)
		{
			throw new NotImplementedException();
		}

		public short GetInt16(int i)
		{
			throw new NotImplementedException();
		}

		public int GetInt32(int i)
		{
			throw new NotImplementedException();
		}

		public long GetInt64(int i)
		{
			throw new NotImplementedException();
		}

		public string GetName(int i)
		{
			throw new NotImplementedException();
		}

		public int GetOrdinal(string name)
		{
			throw new NotImplementedException();
		}

		public DataTable GetSchemaTable()
		{
			throw new NotImplementedException();
		}

		public string GetString(int i)
		{
			throw new NotImplementedException();
		}

		public int GetValues(object[] values)
		{
			throw new NotImplementedException();
		}

		public bool IsDBNull(int i)
		{
			throw new NotImplementedException();
		}

		public bool NextResult()
		{
			throw new NotImplementedException();
		}
	}
}