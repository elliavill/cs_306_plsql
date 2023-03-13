using Microsoft.AspNetCore.Mvc;
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
                try
                {
                   con.Open();
                   OracleCommand cmd = con.CreateCommand();
                   cmd.CommandText = "SELECT * FROM GRADE_TYPE";
                   OracleDataAdapter oda = new OracleDataAdapter(cmd);
                   DataSet dt = new DataSet();
                   oda.Fill(dt);
                   ViewData["showGradeTypes"] = dt.Tables[0];
                }
                catch
                {
                   ViewData["errorMessage"] = "Cannot load information,";
                }
                finally
                {
                   con.Close();
                }
            }
        }

        public void OnPostAddInformation(string gradeTypeCode, string description)
        {
            using (OracleConnection con = new OracleConnection("User ID=cs306_avillyani;Password=StudyDatabaseWithDrSparks;Data Source=CSORACLE"))
            {
                try
                {
                   con.Open();
                   OracleCommand cmd = con.CreateCommand();
                   cmd.CommandText = "INSERT INTO GRADE_TYPE(GRADE_TYPE_CODE, DESCRIPTION) VALUES (:gradeTypeCode, :description)";
                   cmd.Parameters.Add(":gradeTypeCode", HttpContext.Request.Form["gradeTypeCode"].ToString());
                   cmd.Parameters.Add(":description", HttpContext.Request.Form["description"].ToString());
                   cmd.ExecuteNonQuery();
                }
                catch
                {
                    ViewData["errorMessage"] = "Cannot add data. The data is already exist.";
                    OnPostShowInformation();
                }
                finally
                {
                    con.Close();
                }
            }
            OnPostShowInformation();
        }

        public void OnPostDeleteInformation(string gradeTypeRow)
        {
            using (OracleConnection con = new OracleConnection("User ID=cs306_avillyani;Password=StudyDatabaseWithDrSparks;Data Source=CSORACLE"))
            {
                try
                {
                    con.Open();
                    OracleCommand cmd = con.CreateCommand();
                    cmd.CommandText = "DELETE FROM GRADE_TYPE WHERE GRADE_TYPE_CODE = :gradeTypeRow";
                    cmd.Parameters.Add(":gradeTypeRow", gradeTypeRow);
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    ViewData["errorMessage"] = "Cannot delete the record.";
                    OnPostShowInformation();
                }
                finally
                {
                    con.Close();
                }

            }
            OnPostShowInformation();
        }

        public void OnPostEditInformation(string editGradeTypeRow)
        {
            string updatedGradeTypeCode = HttpContext.Request.Form["updatedGradeTypeCode"];
            string updatedDescription = HttpContext.Request.Form["updatedDescription"];

            // Check if the grade type code can be updated
            if (updatedGradeTypeCode == "FI" || updatedGradeTypeCode == "HM" || updatedGradeTypeCode == "MY" ||
                updatedGradeTypeCode == "PA" || updatedGradeTypeCode == "PJ" || updatedGradeTypeCode == "QZ")
            {
                ViewData["errorMessage"] = "The selected grade type code cannot be updated.";
            }
            using (OracleConnection con = new OracleConnection("User ID=cs306_avillyani;Password=StudyDatabaseWithDrSparks;Data Source=CSORACLE"))
            {
                try
                {
                    con.Open();
                    OracleCommand cmd = con.CreateCommand();
                    cmd.CommandText = "UPDATE GRADE_TYPE SET GRADE_TYPE_CODE=:updatedGradeTypeCode, DESCRIPTION=:updatedDescription WHERE GRADE_TYPE_CODE=:editGradeTypeRow";
                    cmd.Parameters.Add(":updatedGradeTypeCode", updatedGradeTypeCode);
                    cmd.Parameters.Add(":updatedDescription", updatedDescription);
                    cmd.Parameters.Add(":editGradeTypeRow", editGradeTypeRow);
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    ViewData["errorMessage"] = "GRADE_TYPE_CODE or DESCRIPTION cannot be empty.";
                    OnPostShowInformation();
                }
                finally
                {
                    con.Close();
                }
            }
            OnPostShowInformation();
        }
    }
}
