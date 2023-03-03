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
               v_last_name VARCHAR2(100) := :last_name; 
               v_count NUMBER(3,0) := 0; 
               CURSOR c_instructors IS SELECT * FROM instructor WHERE last_name LIKE '%' || v_last_name || '%'; 
               v_instructor instructor%ROWTYPE; 
            BEGIN 
               OPEN c_instructors; 
               LOOP 
                  FETCH c_instructors INTO v_instructor; 
                  EXIT WHEN c_instructors%NOTFOUND; 
                  v_count := v_count + 1; 
                  DBMS_OUTPUT.PUT_LINE(v_instructor.last_name);
               END LOOP; 
               CLOSE c_instructors; 
               DBMS_OUTPUT.PUT_LINE('Number of instructors with last name ' || v_last_name || ': ' || v_count); 
            END; 
            ";
            /*cmd.Parameters.Add("last_name", OracleDbType.Varchar2).Value = lastName;*/
            try
            {
               cmd.Parameters.Add("last_name", HttpContext.Request.Form["count"].ToString());
               cmd.ExecuteNonQuery();
               /*instructorCount = Convert.ToInt32(cmd.ExecuteScalar());*/
            }
            catch (OracleException ex)
            {
               ViewData["count"] = ex.Message;
            }
         }
         return instructorCount;
      }
   }
}
