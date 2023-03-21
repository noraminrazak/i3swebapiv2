using I3SwebAPIv2.Class;
using I3SwebAPIv2.Models;
using I3SwebAPIv2.Resources;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Http.Cors;
using System.Web.Http;

namespace I3SwebAPIv2.Controllers
{
    public class LookupsV2Controller : ApiController
    {
        [HttpPost]
        //[Authorize]
        [Route("api/v2/lookup/platform-version")]
        public IHttpActionResult Post_Lookup_Platform_Version([FromBody] SystemVersionParam value)
        {
            LookupDataModel data = new LookupDataModel();

            List<PlatformVersion> list = new List<PlatformVersion>();
            PlatformVersionModel listData = new PlatformVersionModel();
            MySqlTransaction trans = null;
            listData.Success = false;
            string sqlQuery = string.Empty;

            string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    using (MySqlCommand cmd = new MySqlCommand("sp_get_system_version", conn))
                    {
                        cmd.Transaction = trans;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@p_platform_name", value.platform);
                        MySqlDataReader dataReader = cmd.ExecuteReader();

                        if (dataReader.HasRows == true)
                        {
                            while (dataReader.Read())
                            {
                                PlatformVersion prop = new PlatformVersion();
                                prop.platform_name = dataReader["platform_name"].ToString();
                                prop.version_number = dataReader["version_number"].ToString();
                                prop.build_number = dataReader["build_number"].ToString();
                                prop.release = dataReader["release_flag"].ToString();
                                list.Add(prop);
                            }
                            listData.Code = "OK";
                            listData.Message = "Platform exists.";
                            listData.Success = true;
                            listData.Data = list;
                        }
                        else
                        {
                            listData.Success = false;
                            data.Success = true;
                            data.Code = "no_record_found";
                            data.Message = "Platform could not be found.";
                        }

                    }
                }
                catch (Exception ex)
                {
                    listData.Success = false;
                    data.Success = false;
                    data.Code = "error_occured";
                    data.Message = WebApiResources.ErrorOccured;
                    ExceptionUtility.LogException(ex, "lookup/platform-version");
                }
                finally
                {
                    conn.Close();
                }
            }

            if (listData.Success == true)
                return Ok(listData);

            return Ok(data);
        }

        [HttpPost]
        //[Authorize]
        [Route("api/v2/lookup/country")]
        public IHttpActionResult Post_Lookup_Country()
        {
            LookupDataModel data = new LookupDataModel();

            List<Country> list = new List<Country>();
            CountryModel listData = new CountryModel();
            MySqlTransaction trans = null;
            listData.Success = false;
            string sqlQuery = string.Empty;

            string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    using (MySqlCommand cmd = new MySqlCommand("sp_get_country", conn))
                    {
                        cmd.Transaction = trans;
                        cmd.CommandType = CommandType.StoredProcedure;
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows == true)
                        {
                            while (dataReader.Read())
                            {
                                Country prop = new Country();
                                prop.country_id = Convert.ToInt16(dataReader["country_id"]);
                                prop.country_name = dataReader["country_name"].ToString();
                                prop.locale_code = dataReader["locale_code"].ToString();
                                prop.country_code = dataReader["country_code"].ToString();
                                list.Add(prop);
                            }
                            listData.Success = true;
                            listData.Code = "select_country";
                            listData.Message = "Please select country.";
                            listData.Data = list;
                        }
                        else
                        {
                            //listData.Success = false;
                            listData.Success = true;
                            listData.Code = "no_record_found";
                            listData.Message = "No record found. Please contact our customer support.";
                            listData.Data = list;
                        }

                    }
                }
                catch (Exception ex)
                {
                    //listData.Success = false;
                    data.Success = false;
                    data.Code = "error_occured";
                    data.Message = WebApiResources.ErrorOccured;
                    ExceptionUtility.LogException(ex, "lookup/country");
                }
                finally
                {
                    conn.Close();
                }
            }

            if (listData.Success == true)
                return Ok(listData);

            return Ok(data);
        }

        [HttpPost]
        //[Authorize]
        [Route("api/v2/lookup/nationality")]
        public IHttpActionResult Post_Lookup_Nationality()
        {
            LookupDataModel data = new LookupDataModel();

            List<Country> list = new List<Country>();
            CountryModel listData = new CountryModel();
            MySqlTransaction trans = null;
            listData.Success = false;
            string sqlQuery = string.Empty;

            string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    using (MySqlCommand cmd = new MySqlCommand("sp_get_nationality", conn))
                    {
                        cmd.Transaction = trans;
                        cmd.CommandType = CommandType.StoredProcedure;
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows == true)
                        {
                            while (dataReader.Read())
                            {
                                Country prop = new Country();
                                prop.country_id = Convert.ToInt16(dataReader["country_id"]);
                                prop.country_name = dataReader["country_name"].ToString();
                                prop.locale_code = dataReader["locale_code"].ToString();
                                prop.country_code = dataReader["country_code"].ToString();
                                list.Add(prop);
                            }
                            listData.Success = true;
                            listData.Code = "select_country";
                            listData.Message = "Please select country.";
                            listData.Data = list;
                        }
                        else
                        {
                            //listData.Success = false;
                            listData.Success = true;
                            listData.Code = "no_record_found";
                            listData.Message = "No record found. Please contact our customer support.";
                            listData.Data = list;
                        }

                    }
                }
                catch (Exception ex)
                {
                    //listData.Success = false;
                    data.Success = false;
                    data.Code = "error_occured";
                    data.Message = WebApiResources.ErrorOccured;
                    ExceptionUtility.LogException(ex, "lookup/country");
                }
                finally
                {
                    conn.Close();
                }
            }

            if (listData.Success == true)
                return Ok(listData);

            return Ok(data);
        }

        [HttpPost]
        //[Authorize]
        [Route("api/v2/lookup/state")]
        public IHttpActionResult Post_Lookup_State()
        {
            LookupDataModel data = new LookupDataModel();

            List<State> list = new List<State>();
            StateModel listData = new StateModel();
            MySqlTransaction trans = null;
            listData.Success = false;
            string sqlQuery = string.Empty;

            string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    using (MySqlCommand cmd = new MySqlCommand("sp_get_state", conn))
                    {
                        cmd.Transaction = trans;
                        cmd.CommandType = CommandType.StoredProcedure;
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows == true)
                        {
                            while (dataReader.Read())
                            {
                                State prop = new State();
                                prop.state_id = Convert.ToInt16(dataReader["state_id"]);
                                prop.state_name = dataReader["state_name"].ToString();
                                list.Add(prop);
                            }
                            listData.Success = true;
                            listData.Code = "select_state";
                            listData.Message = "Please select state.";
                            listData.Data = list;
                        }
                        else
                        {
                            //listData.Success = false;
                            listData.Success = true;
                            listData.Code = "no_record_found";
                            listData.Message = "No record found. Please contact our customer support.";
                            listData.Data = list;
                        }

                    }
                }
                catch (Exception ex)
                {
                    //listData.Success = false;
                    data.Success = false;
                    data.Code = "error_occured";
                    data.Message = WebApiResources.ErrorOccured;
                    ExceptionUtility.LogException(ex, "lookup/state");
                }
                finally
                {
                    conn.Close();
                }
            }

            if (listData.Success == true)
                return Ok(listData);

            return Ok(data);    
        }

        [HttpPost]
        //[Authorize]
        [Route("api/v2/lookup/city")]
        public IHttpActionResult Post_Lookup_City([FromBody] StateParam value)
        {
            LookupDataModel data = new LookupDataModel();
            if (value.state_id != 0)
            {
                List<City> list = new List<City>();
                CityModel listData = new CityModel();
                MySqlTransaction trans = null;
                listData.Success = false;
                string sqlQuery = string.Empty;

                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_city", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_state_id", value.state_id);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    City prop = new City();
                                    prop.city_id = Convert.ToInt16(dataReader["city_id"]);
                                    prop.city_name = dataReader["city"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "select_city";
                                listData.Message = "Please select city.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "No record found. Please contact our customer support.";
                                listData.Data = list;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        //listData.Success = false;
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "lookup/city");
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

                if (listData.Success == true)
                    return Ok(listData);

                return Ok(data);
            }
            else
            {
                return BadRequest("Missing parameters.");
            }
        }

        [HttpPost]
        //[Authorize]
        [Route("api/v2/lookup/card-types")]
        public IHttpActionResult Post_Lookup_Card_Types()
        {
            LookupDataModel data = new LookupDataModel();

            List<CardType> list = new List<CardType>();
            CardTypeModel listData = new CardTypeModel();
            MySqlTransaction trans = null;
            listData.Success = false;
            string sqlQuery = string.Empty;

            string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    using (MySqlCommand cmd = new MySqlCommand("sp_get_card_type", conn))
                    {
                        cmd.Transaction = trans;
                        cmd.CommandType = CommandType.StoredProcedure;
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows == true)
                        {
                            while (dataReader.Read())
                            {
                                CardType prop = new CardType();
                                prop.card_type_id = Convert.ToInt16(dataReader["card_type_id"]);
                                prop.card_type = dataReader["card_type"].ToString();
                                prop.card_type_bm = dataReader["card_type_bm"].ToString();
                                list.Add(prop);
                            }
                            listData.Success = true;
                            listData.Code = "select_card_type";
                            listData.Message = "Please select card type.";
                            listData.Data = list;
                        }
                        else
                        {
                            //listData.Success = false;
                            listData.Success = true;
                            listData.Code = "no_record_found";
                            listData.Message = "No record found. Please contact our customer support.";
                            listData.Data = list;
                        }

                    }
                }
                catch (Exception ex)
                {
                    //listData.Success = false;
                    data.Success = false;
                    data.Code = "error_occured";
                    data.Message = WebApiResources.ErrorOccured;
                    ExceptionUtility.LogException(ex, "lookup/card-types");
                }
                finally
                {
                    conn.Close();
                }
            }

            if (listData.Success == true)
                return Ok(listData);

            return Ok(data);
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/lookup/card-status")]
        public IHttpActionResult Post_Lookup_Card_Status()
        {
            LookupDataModel data = new LookupDataModel();

            List<CardStatus> list = new List<CardStatus>();
            CardStatusModel listData = new CardStatusModel();
            MySqlTransaction trans = null;
            listData.Success = false;
            string sqlQuery = string.Empty;

            string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    using (MySqlCommand cmd = new MySqlCommand("sp_get_card_status", conn))
                    {
                        cmd.Transaction = trans;
                        cmd.CommandType = CommandType.StoredProcedure;
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows == true)
                        {
                            while (dataReader.Read())
                            {
                                CardStatus prop = new CardStatus();
                                prop.card_status_id = Convert.ToInt16(dataReader["card_status_id"]);
                                prop.card_status = dataReader["card_status"].ToString();
                                prop.card_status_bm = dataReader["card_status_bm"].ToString();
                                list.Add(prop);
                            }
                            listData.Success = true;
                            listData.Code = "select_card_type";
                            listData.Message = "Please select card status.";
                            listData.Data = list;
                        }
                        else
                        {
                            //listData.Success = false;
                            //listData.Success = true;
                            //listData.Code = "no_record_found";
                            listData.Message = "No record found. Please contact our customer support.";
                            listData.Data = list;
                        }

                    }
                }
                catch (Exception ex)
                {
                    //listData.Success = false;
                    data.Success = false;
                    data.Code = "error_occured";
                    data.Message = WebApiResources.ErrorOccured;
                    ExceptionUtility.LogException(ex, "lookup/card-status");
                }
                finally
                {
                    conn.Close();
                }
            }

            if (listData.Success == true)
                return Ok(listData);

            return Ok(data);
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/lookup/staff-types")]
        public IHttpActionResult Post_Lookup_Staff_Types()
        {
            LookupDataModel data = new LookupDataModel();

            List<StaffType> list = new List<StaffType>();
            StaffTypeModel listData = new StaffTypeModel();
            MySqlTransaction trans = null;
            listData.Success = false;
            string sqlQuery = string.Empty;

            string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    using (MySqlCommand cmd = new MySqlCommand("sp_get_staff_type", conn))
                    {
                        cmd.Transaction = trans;
                        cmd.CommandType = CommandType.StoredProcedure;
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows == true)
                        {
                            while (dataReader.Read())
                            {
                                StaffType prop = new StaffType();
                                prop.staff_type_id = Convert.ToInt16(dataReader["staff_type_id"]);
                                prop.staff_type = dataReader["staff_type"].ToString();
                                prop.staff_type_bm = dataReader["staff_type_bm"].ToString();
                                list.Add(prop);
                            }
                            listData.Success = true;
                            listData.Code = "select_staff_type";
                            listData.Message = "Please select staff types.";
                            listData.Data = list;
                        }
                        else
                        {
                            //listData.Success = false;
                            listData.Success = true;
                            listData.Code = "no_record_found";
                            listData.Message = "No record found. Please contact our customer support.";
                            listData.Data = list;
                        }

                    }
                }
                catch (Exception ex)
                {
                    //listData.Success = false;
                    data.Success = false;
                    data.Code = "error_occured";
                    data.Message = WebApiResources.ErrorOccured;
                    ExceptionUtility.LogException(ex, "lookup/staff-types");
                }
                finally
                {
                    conn.Close();
                }
            }

            if (listData.Success == true)
                return Ok(listData);

            return Ok(data);
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/lookup/attendance")]
        public IHttpActionResult Post_Lookup_Attendance()
        {
            LookupDataModel data = new LookupDataModel();
            List<Attendance> list = new List<Attendance>();
            AttendanceModel listData = new AttendanceModel();
            MySqlTransaction trans = null;
            listData.Success = false;
            string sqlQuery = string.Empty;

            string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    using (MySqlCommand cmd = new MySqlCommand("sp_get_attendance", conn))
                    {
                        cmd.Transaction = trans;
                        cmd.CommandType = CommandType.StoredProcedure;
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows == true)
                        {
                            while (dataReader.Read())
                            {
                                Attendance prop = new Attendance();
                                prop.attendance_id = Convert.ToInt16(dataReader["attendance_id"]);
                                prop.attendance_code = dataReader["attendance_code"].ToString();
                                prop.attendance = dataReader["attendance"].ToString();
                                list.Add(prop);
                            }
                            listData.Success = true;
                            listData.Code = "select_attendance";
                            listData.Message = "Please select attendance.";
                            listData.Data = list;
                        }
                        else
                        {
                            //listData.Success = false;
                            listData.Success = true;
                            listData.Code = "no_record_found";
                            listData.Message = "No record found. Please contact our customer support.";
                            listData.Data = list;
                        }

                    }
                }
                catch (Exception ex)
                {
                    //listData.Success = false;
                    data.Success = false;
                    data.Code = "error_occured";
                    data.Message = WebApiResources.ErrorOccured;
                    ExceptionUtility.LogException(ex, "lookup/attendance");
                }
                finally
                {
                    conn.Close();
                }
            }

            if (listData.Success == true)
                return Ok(listData);

            return Ok(data);
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/lookup/reason-for-absent")]
        public IHttpActionResult Post_Lookup_Reason_For_Absent()
        {
            LookupDataModel data = new LookupDataModel();

            List<Reason> list = new List<Reason>();
            ReasonModel listData = new ReasonModel();
            MySqlTransaction trans = null;
            listData.Success = false;
            string sqlQuery = string.Empty;

            string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    using (MySqlCommand cmd = new MySqlCommand("sp_get_reason_for_absent", conn))
                    {
                        cmd.Transaction = trans;
                        cmd.CommandType = CommandType.StoredProcedure;
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows == true)
                        {
                            while (dataReader.Read())
                            {
                                Reason prop = new Reason();
                                prop.reason_id = Convert.ToInt16(dataReader["reason_id"]);
                                prop.reason_for_absent = dataReader["reason_for_absent"].ToString();
                                prop.reason_for_absent_bm = dataReader["reason_for_absent_bm"].ToString();
                                list.Add(prop);
                            }
                            listData.Success = true;
                            listData.Code = "select_reason_for_absent";
                            listData.Message = "Please select reason for absent.";
                            listData.Data = list;
                        }
                        else
                        {
                            //listData.Success = false;
                            listData.Success = true;
                            listData.Code = "no_record_found";
                            listData.Message = "No record found. Please contact our customer support.";
                            listData.Data = list;
                        }

                    }
                }
                catch (Exception ex)
                {
                    //listData.Success = false;
                    data.Success = false;
                    data.Code = "error_occured";
                    data.Message = WebApiResources.ErrorOccured;
                    ExceptionUtility.LogException(ex, "lookup/reason-for-absent");
                }
                finally
                {
                    conn.Close();
                }
            }

            if (listData.Success == true)
                return Ok(listData);

            return Ok(data);
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/lookup/user-race")]
        public IHttpActionResult Post_Lookup_User_Race()
        {
            LookupDataModel data = new LookupDataModel();

            List<UserRace> list = new List<UserRace>();
            UserRaceModel listData = new UserRaceModel();
            MySqlTransaction trans = null;
            listData.Success = false;
            string sqlQuery = string.Empty;

            string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    using (MySqlCommand cmd = new MySqlCommand("sp_get_user_race", conn))
                    {
                        cmd.Transaction = trans;
                        cmd.CommandType = CommandType.StoredProcedure;
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows == true)
                        {
                            while (dataReader.Read())
                            {
                                UserRace prop = new UserRace();
                                prop.user_race_id = Convert.ToInt16(dataReader["user_race_id"]);
                                prop.user_race = dataReader["user_race"].ToString();
                                prop.user_race_bm = dataReader["user_race_bm"].ToString();
                                list.Add(prop);
                            }
                            listData.Success = true;
                            listData.Code = "select_race";
                            listData.Message = "Please select race.";
                            listData.Data = list;
                        }
                        else
                        {
                            //listData.Success = false;
                            listData.Success = true;
                            listData.Code = "no_record_found";
                            listData.Message = "No record found. Please contact our customer support.";
                            listData.Data = list;
                        }

                    }
                }
                catch (Exception ex)
                {
                    //listData.Success = false;
                    data.Success = false;
                    data.Code = "error_occured";
                    data.Message = WebApiResources.ErrorOccured;
                    ExceptionUtility.LogException(ex, "lookup/user-race");
                }
                finally
                {
                    conn.Close();
                }
            }

            if (listData.Success == true)
                return Ok(listData);

            return Ok(data);
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/lookup/merchant-types")]
        public IHttpActionResult Post_Lookup_Merchant_Types()
        {
            LookupDataModel data = new LookupDataModel();

            List<MerchantType> list = new List<MerchantType>();
            MerchantTypeModel listData = new MerchantTypeModel();
            MySqlTransaction trans = null;
            listData.Success = false;
            string sqlQuery = string.Empty;

            string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    using (MySqlCommand cmd = new MySqlCommand("sp_get_merchant_type", conn))
                    {
                        cmd.Transaction = trans;
                        cmd.CommandType = CommandType.StoredProcedure;
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows == true)
                        {
                            while (dataReader.Read())
                            {
                                MerchantType prop = new MerchantType();
                                prop.merchant_type_id = Convert.ToInt16(dataReader["merchant_type_id"]);
                                prop.merchant_type = dataReader["merchant_type"].ToString();
                                prop.merchant_type_bm = dataReader["merchant_type_bm"].ToString();
                                list.Add(prop);
                            }
                            listData.Success = true;
                            listData.Code = "select_merchant_type";
                            listData.Message = "Please select merchant types.";
                            listData.Data = list;
                        }
                        else
                        {
                            //listData.Success = false;
                            listData.Success = true;
                            listData.Code = "no_record_found";
                            listData.Message = "No record found. Please contact our customer support.";
                            listData.Data = list;
                        }

                    }
                }
                catch (Exception ex)
                {
                    //listData.Success = false;
                    data.Success = false;
                    data.Code = "error_occured";
                    data.Message = WebApiResources.ErrorOccured;
                    ExceptionUtility.LogException(ex, "lookup/merchant-types");
                }
                finally
                {
                    conn.Close();
                }
            }

            if (listData.Success == true)
                return Ok(listData);

            return Ok(data);
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/lookup/problem-types")]
        public IHttpActionResult Post_Lookup_Problem_Types()
        {

            ProblemTypeModel data = new ProblemTypeModel();

            List<ProblemType> list = new List<ProblemType>();
            ProblemTypeModel listData = new ProblemTypeModel();
            MySqlTransaction trans = null;
            listData.Success = false;
            string sqlQuery = string.Empty;

            string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    using (MySqlCommand cmd = new MySqlCommand("sp_get_problem_type_special", conn))
                    {
                        cmd.Transaction = trans;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@p_support_category_id", 7);
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows == true)
                        {
                            while (dataReader.Read())
                            {
                                ProblemType prop = new ProblemType();
                                prop.problem_type_id = Convert.ToInt16(dataReader["problem_type_id"]);
                                prop.problem_type = dataReader["problem_type"].ToString();
                                prop.problem_type_bm = dataReader["problem_type_bm"].ToString();
                                list.Add(prop);
                            }
                            listData.Success = true;
                            listData.Code = "select_problem_type";
                            listData.Message = "Please select problem types.";
                            listData.Data = list;
                        }
                        else
                        {
                            //listData.Success = false;
                            listData.Success = true;
                            listData.Code = "no_record_found";
                            listData.Message = "No record found. Please contact our customer support.";
                            listData.Data = list;
                        }

                    }
                }
                catch (Exception ex)
                {
                    //listData.Success = false;
                    data.Success = false;
                    data.Code = "error_occured";
                    data.Message = WebApiResources.ErrorOccured;
                    ExceptionUtility.LogException(ex, "lookup/problem-types");
                }
                finally
                {
                    conn.Close();
                }
            }

            if (listData.Success == true)
                return Ok(listData);

            return Ok(data);
        }

        [HttpPost]
        //[Authorize]
        [Route("api/v2/lookup/occupation")]
        public IHttpActionResult Post_Lookup_Occupation()
        {
            ProblemTypeModel data = new ProblemTypeModel();

            List<Occupation> list = new List<Occupation>();
            OccupationModel listData = new OccupationModel();
            MySqlTransaction trans = null;
            listData.Success = false;
            string sqlQuery = string.Empty;

            string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    using (MySqlCommand cmd = new MySqlCommand("sp_get_occupation", conn))
                    {
                        cmd.Transaction = trans;
                        cmd.CommandType = CommandType.StoredProcedure;
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows == true)
                        {
                            while (dataReader.Read())
                            {
                                Occupation prop = new Occupation();
                                prop.occupation_id = Convert.ToInt16(dataReader["occupation_id"]);
                                prop.occupation = dataReader["occupation_name"].ToString();
                                list.Add(prop);
                            }
                            listData.Success = true;
                            listData.Code = "select_occupation";
                            listData.Message = "Please select occupation.";
                            listData.Data = list;
                        }
                        else
                        {
                            //listData.Success = false;
                            listData.Success = true;
                            listData.Code = "no_record_found";
                            listData.Message = "No record found. Please contact our customer support.";
                            listData.Data = list;
                        }

                    }
                }
                catch (Exception ex)
                {
                    //listData.Success = false;
                    data.Success = false;
                    data.Code = "error_occured";
                    data.Message = WebApiResources.ErrorOccured;
                    ExceptionUtility.LogException(ex, "lookup/occupation");
                }
                finally
                {
                    conn.Close();
                }
            }

            if (listData.Success == true)
                return Ok(listData);

            return Ok(data);
        }
    }
}
