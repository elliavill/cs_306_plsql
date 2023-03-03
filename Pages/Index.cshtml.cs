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
                                v_last_name VARCHAR2(100) := :last_name;  
                                v_count NUMBER(3,0) := 0; 
                            BEGIN  
                                FOR v_instructor IN (SELECT * FROM instructor WHERE last_name LIKE '%' || :last_name || '%') LOOP  
                                    v_count := v_count + 1;    
                                END LOOP;  
                                raise_application_error(-20000, 'Number of instructors that have that last name or that version is ' || v_count);
                            END;";
            try
            {
               cmd.Parameters.Add("Number of instructors that have that last name or that version is ", HttpContext.Request.Form["get_words"].ToString());
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
