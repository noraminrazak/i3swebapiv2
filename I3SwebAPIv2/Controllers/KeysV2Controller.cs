
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Web.Http;
using static I3SwebAPIv2.Class.Crypt;

namespace I3SwebAPIv2.Controllers
{
    public class KeysV2Controller : ApiController
    {
        [HttpGet]
        [Route("api/v2/keys/generate")]
        public IHttpActionResult Get_Key_Generate()
        {
            RSAKeysTypes data = new RSAKeysTypes();
            var keyGenerator = new RSACryptographyKeyGenerator();
            var keys = keyGenerator.GenerateKeys(RSAKeySize.Key512);

            string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(constr))
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_insert_key", conn))
                {
                    MySqlTransaction trans;
                    trans = conn.BeginTransaction();
                    cmd.Transaction = trans;

                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_public_key", keys.PublicKey);
                        cmd.Parameters.AddWithValue("@p_private_key", keys.PrivateKey);
                        cmd.ExecuteNonQuery();
                        trans.Commit();

                        data.PublicKey = keys.PublicKey;
                        data.PrivateKey = keys.PrivateKey;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return Ok(data);
        }
    }
}
