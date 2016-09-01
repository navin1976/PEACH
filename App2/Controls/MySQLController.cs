//using System;  // C# , ADO.NET  
//using System.Diagnostics;
//using C = System.Data; // System.Data.dll  

//namespace ProofOfConcept_SQL_CSharp
//{
//    public class Program
//    {
//        static public void Main()
//        {
//            using (var connection = new C.(
//                "Server=tcp:peach2016visualreport.database.windows.net,1433;Initial Catalog=peach2016visualreport;Persist Security Info=False;User ID={peach2016visualreportapp};Password={Visualreportapp11};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
//                ))
//            {
//                connection.Open();
//                Debug.WriteLine("Connected successfully.");

//                Debug.WriteLine("Press any key to finish...");
//            }
//        }
//    }
//}