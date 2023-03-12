﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DOOM.Pages
{
    public class IndexModel : PageModel
    {

        public void OnGet()
        {
            OnPostShowInformation();
        }

        public void OnPostShowInformation()
        {
            using (OracleConnection con = new OracleConnection("User ID=cs306_avillyani;Password=StudyDatabaseWithDrSparks;Data Source=CSORACLE"))
            {
                con.Open();
                OracleCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT * FROM GRADE_TYPE";
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                DataSet dt = new DataSet();
                oda.Fill(dt);
                ViewData["myStuff"] = dt.Tables[0];
            }
        }

        public void OnPostAddInformation(string gradeTypeCode, string description)
        {
            using (OracleConnection con = new OracleConnection("User ID=cs306_avillyani;Password=StudyDatabaseWithDrSparks;Data Source=CSORACLE"))
            {
                con.Open();
                OracleCommand cmd = con.CreateCommand();
                cmd.CommandText = "INSERT INTO GRADE_TYPE(GRADE_TYPE_CODE, DESCRIPTION) VALUES (:gradeTypeCode, :description)";
                cmd.Parameters.Add(":gradeTypeCode", HttpContext.Request.Form["gradeTypeCode"].ToString());
                cmd.Parameters.Add(":description", HttpContext.Request.Form["description"].ToString());
                cmd.ExecuteNonQuery();
            }
            OnPostShowInformation();
        }

        public void OnPostDeleteInformation(string gradeTypeRow)
        {
            using (OracleConnection con = new OracleConnection("User ID=cs306_avillyani;Password=StudyDatabaseWithDrSparks;Data Source=CSORACLE"))
            {
                con.Open();
                OracleCommand cmd = con.CreateCommand();
                cmd.CommandText = "DELETE FROM GRADE_TYPE WHERE GRADE_TYPE_CODE = :gradeTypeRow";
                cmd.Parameters.Add(":gradeTypeRow", gradeTypeRow);
                cmd.ExecuteNonQuery();
            }
            OnPostShowInformation();
        }

        public void OnPostEditInformation(string editGradeTypeRow)
        {
            using (OracleConnection con = new OracleConnection("User ID=cs306_avillyani;Password=StudyDatabaseWithDrSparks;Data Source=CSORACLE"))
            {
                con.Open();
                OracleCommand cmd = con.CreateCommand();
                cmd.CommandText = "UPDATE GRADE_TYPE SET grade_type_code = :updatedGradeTypeCode, description = :updatedDescription, modified_by = USER, modified_date = SYSDATE WHERE grade_type_code = :editGradeTypeRow";
                cmd.Parameters.Add(":updatedGradeTypeCode", HttpContext.Request.Form["updatedGradeTypeCode"].ToString());
                cmd.Parameters.Add(":updatedDescription", HttpContext.Request.Form["updatedDescription"].ToString());
                cmd.Parameters.Add(":editGradeTypeRow", editGradeTypeRow);
                cmd.ExecuteNonQuery();
            }
            OnPostShowInformation();
        }
    }
}