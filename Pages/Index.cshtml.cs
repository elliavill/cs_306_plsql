using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOOM.Pages
{
   public class IndexModel : PageModel
   {
      [BindProperty]
      public string LastName { get; set; }
      public int InstructorCount { get; set; }

      public void OnGet()
      {

      }

      public void OnPost()
      {
         InstructorCount = RunPLSQLBlock(LastName);
      }

      public int RunPLSQLBlock(string lastName)
      {
         int instructorCount = 0;
         using (OracleConnection con = new OracleConnection("User ID=cs306_avillyani;Password=StudyDatabaseWithDrSparks;Data Source=CSORACLE"))
         {
            con.Open();
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = @" 
            DECLARE 
                v_last_name VARCHAR2(100); 
                CURSOR instructor_cur IS 
                    SELECT COUNT(*) AS instructor_count 
                    FROM instructors 
                    WHERE UPPER(last_name) LIKE '%' || UPPER(v_last_name) || '%'; 
            BEGIN 
                v_last_name := :last_name; 
                OPEN instructor_cur; 
                FETCH instructor_cur INTO instructor_count; 
                CLOSE instructor_cur; 
            END; 
            ";
            cmd.Parameters.Add("last_name", OracleDbType.Varchar2).Value = lastName;
            try
            {
               instructorCount = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (OracleException ex)
            {
               // Handle the exception
            }
         }
         return instructorCount;
      }
   }
}
