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
      public void OnGet()
      {

      }

      public void OnPostCount()
      {
         using (OracleConnection con = new OracleConnection("User ID=cs306_avillyani;Password=StudyDatabaseWithDrSparks;Data Source=CSORACLE"))
         {
            con.Open();
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = @"DECLARE
                                 CURSOR c_instructors IS SELECT * FROM instructor WHERE last_name LIKE '%' || :last_name || '%'; 
                                 v_count NUMBER(3,0) := 0; 
                               BEGIN  
                                 FOR v_instructor IN c_instructors LOOP  
                                     v_count := v_count + 1;    
                                 END LOOP;  
                                 raise_application_error(-20000, 'Number os the instructor with ' || :last_name || ' is ' || v_count);
                               END;";
            try
            {
               cmd.Parameters.Add("Number of instructors that have that letters ",
                  HttpContext.Request.Form["letter_in_last_name"].ToString());
               cmd.ExecuteNonQuery();
            }
            catch (OracleException ex)
            {
               ViewData["count"] = ex.Message;
            }
         }
      }
   }
}
