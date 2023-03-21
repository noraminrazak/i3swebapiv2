using I3SwebAPIv2.Class;
using I3SwebAPIv2.Models;
using I3SwebAPIv2.Resources;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;

namespace I3SwebAPIv2.Controllers
{
    public class GeneralV2Controller : ApiController
    {
        [HttpPost]
        [Authorize]
        [Route("api/v2/general/leave-club")]
        public IHttpActionResult Post_General_Leave_Club(string culture, [FromBody]RemoveClubRelationship value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            StaffDataModel data = new StaffDataModel();
            if (value.relationship_id != 0 && value.profile_id != 0 && value.club_id != 0 && value.update_by != null)
            {
                MySqlTransaction trans = null;
                data.Success = false;
                string sqlQuery = string.Empty;
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_delete_school_club_relationship", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_relationship_id", value.relationship_id);
                            cmd.Parameters.AddWithValue("@p_club_id", value.club_id);
                            cmd.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            var status_code = cmd.Parameters["@p_status_code"].Value.ToString();

                            if (status_code == "delete_success")
                            {
                                data.Success = true;
                                data.Code = "delete_success";
                                data.Message = WebApiResources.ClubRelationshipRemoveSuccess;
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "no_record_found";
                                data.Message = WebApiResources.ClubRelationshipNotFound;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "staff/leave-club");
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

                return Ok(data);
            }
            else
            {
                return BadRequest("Missing parameters.");
            }
        }
    }
}
