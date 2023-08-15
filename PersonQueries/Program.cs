using System;
using System.Data.SqlClient;
using System.Diagnostics;

namespace PersonQueries
{
	internal class Program
	{
		static readonly SqlConnection conn = new SqlConnection("Server=.\\SQLEXPRESS;Database=Test;Trusted_Connection=True;");
		static readonly Random ran = new Random();

		static bool CreateTable()
		{
			SqlCommand cmd = conn.CreateCommand();

			try
			{
				conn.Open();

				cmd.CommandText = "DROP TABLE IF EXISTS Person";
				cmd.ExecuteNonQuery();
				cmd.CommandText = "CREATE TABLE Person (fullname VARCHAR(50), dob DATE, sex CHAR(1))";
				cmd.ExecuteNonQuery();

				return true;
			}
			finally
			{
				cmd.Dispose();
				conn.Close();
			}
		}

		static bool InsertRow(string fullname, DateTime dob, char sex)
		{
			SqlCommand cmd = conn.CreateCommand();

			try
			{
				conn.Open();

				cmd.CommandText = "INSERT INTO Person VALUES (@fullname, @dob, @sex)";
				cmd.Parameters.AddWithValue("@fullname", fullname);
				cmd.Parameters.AddWithValue("@dob", dob.Date);
				cmd.Parameters.AddWithValue("@sex", sex);
				cmd.ExecuteNonQuery();

				return true;
			}
			finally
			{
				cmd.Dispose();
				conn.Close();
			}
		}

		static bool InsertRandomBulk()
		{
			SqlBulkCopy loader = new SqlBulkCopy(conn);
			loader.DestinationTableName = "Person";
			loader.ColumnMappings.Add(0, "fullname");
			loader.ColumnMappings.Add(1, "dob");
			loader.ColumnMappings.Add(2, "sex");

			try
			{
				conn.Open();

				RandomReader bulk = new RandomReader(ran, 1000000);
				loader.WriteToServer(bulk);
				bulk = new RandomReader(ran, 100, true);
				loader.WriteToServer(bulk);

				return true;
			}
			finally
			{
				loader.Close();
				conn.Close();
			}
		}

		static bool FirstSelect()
		{
			SqlCommand cmd = conn.CreateCommand();

			try
			{
				conn.Open();

				cmd.CommandText = "SELECT *, " +
					"DATEDIFF(YEAR, p.dob, GETDATE()) - CASE WHEN DATEADD(YEAR, DATEDIFF(YEAR, p.dob, GETDATE()), p.dob) > GETDATE() THEN 1 ELSE 0 END AS age " +
					"FROM Person p " +
					"WHERE EXISTS (" +
					"SELECT base.* FROM (" +
					"SELECT fullname, dob FROM Person GROUP BY fullname, dob HAVING COUNT (*) = 1" +
					") base " +
					"WHERE p.fullname = base.fullname and p.dob = base.dob)";

				using (SqlDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						Console.WriteLine($"{reader[0]}\t{(DateTime)reader[1]:dd.MM.yyyy}\t{reader[2]}\t{reader[3]}");
					}
				}

				return true;
			}
			finally
			{
				cmd.Dispose();
				conn.Close();
			}
		}

		static bool SecondSelect()
		{
			SqlCommand cmd = conn.CreateCommand();
			Stopwatch stopwatch = new Stopwatch();

			try
			{
				conn.Open();
				cmd.CommandText = "SELECT * FROM Person WHERE fullname LIKE 'F%' AND sex = 'm'";
				stopwatch.Start();

				using (SqlDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						Console.WriteLine($"{reader[0]}\t{(DateTime)reader[1]:dd.MM.yyyy}\t{reader[2]}");
					}
				}

				stopwatch.Stop();
				Console.WriteLine($"Query executed in (ms): {stopwatch.ElapsedMilliseconds}");
				return true;
			}
			finally
			{
				cmd.Dispose();
				conn.Close();
			}
		}

		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Console.WriteLine("No method provided.");
				return;
			}

			try
			{
				switch (args[0])
				{
					case "1":
						if (CreateTable()) Console.WriteLine("Table created.");
						break;
					case "2":
						bool ok = InsertRow(args[1], Convert.ToDateTime(args[2]), Convert.ToChar(args[3]));
						if (ok) Console.WriteLine("Row inserted.");
						break;
					case "3":
						FirstSelect();
						break;
					case "4":
						if (InsertRandomBulk()) Console.WriteLine("Bulk inserted.");
						break;
					case "5":
						SecondSelect();
						break;
					default:
						Console.WriteLine("No method provided.");
						break;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}