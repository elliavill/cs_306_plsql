﻿using Microsoft.AspNetCore.Mvc;
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
                                 CURSOR c_instructors IS SELECT * FROM instructor WHERE last_name LIKE '%' || :last_name_pattern || '%'; 
                                 v_count NUMBER(3,0) := 0; 
                               BEGIN  
                                 FOR v_instructor IN c_instructors LOOP  
                                     v_count := v_count + 1;    
                                 END LOOP;
                                 IF v_count = 0 THEN
                                    raise_application_error(-20000, 'No instructors found with the given search criteria.');
                                 ELSE
                                    raise_application_error(-20000, 'Number of instructors with ' || :last_name_pattern || ' in their last name: ' || v_count);
                                 END IF;
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