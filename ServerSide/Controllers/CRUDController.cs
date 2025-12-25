using Azure;
using CrudApi.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Npgsql;
using System.Data;
//using Microsoft System.Data.SqlClient;
using System.Data.SqlClient;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Response = CrudApi.models.Response;


namespace CrudApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CRUDController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public CRUDController(IConfiguration iconfiguration)
        {
            _configuration = iconfiguration;
        }


        //postgresql DB

        [HttpGet]
        [Route("GetAllLists")]
        public string GetList()
        {
            NpgsqlConnection conn = new NpgsqlConnection(
                _configuration.GetConnectionString("CrudAppconn"));

            string query = "SELECT * FROM crud_data";

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<crud> crudlist = new List<crud>();
            Response response = new Response();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    crud _crud = new crud();

                    _crud.id = Convert.ToInt32(dt.Rows[i]["id"]);
                    _crud.name = Convert.ToString(dt.Rows[i]["name"]);
                    _crud.description = Convert.ToString(dt.Rows[i]["description"]);
                    _crud.price = Convert.ToDouble(dt.Rows[i]["price"]);
                    _crud.quantity = Convert.ToInt32(dt.Rows[i]["quantity"]);

                    crudlist.Add(_crud);
                }
            }

            if (crudlist.Count > 0)
            {
                return JsonConvert.SerializeObject(crudlist);
            }
            else
            {
                response.StatusCode = 100;
                response.ErrorMessage = "No record Found";
                return JsonConvert.SerializeObject(response);
            }
        }



        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            using var conn = new NpgsqlConnection(
                _configuration.GetConnectionString("CrudAppconn"));

            string query = "SELECT * FROM crud_data WHERE id = @Id";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, conn);
            da.SelectCommand.Parameters.AddWithValue("@Id", id);

            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
                return NotFound("Record not found");

            crud crud = new crud
            {
                id = Convert.ToInt32(dt.Rows[0]["id"]),
                name = Convert.ToString(dt.Rows[0]["name"]),
                description = Convert.ToString(dt.Rows[0]["description"]),
                price = Convert.ToDouble(dt.Rows[0]["price"]),
                quantity = Convert.ToInt32(dt.Rows[0]["quantity"])
            };

            return Ok(crud);
        }




        [HttpPost]
        [Route("PostList")]
        public IActionResult PostList([FromBody] crud obj)
        {
            if (obj == null)
                return BadRequest("No data received");

            using var conn = new NpgsqlConnection(
                _configuration.GetConnectionString("CrudAppconn"));

            conn.Open();

            string maxQuery = "SELECT COALESCE(MAX(id),0) FROM crud_data";
            NpgsqlCommand cmd = new NpgsqlCommand(maxQuery, conn);
            int id = Convert.ToInt32(cmd.ExecuteScalar()) + 1;

            string insertQuery = @"INSERT INTO crud_data
        (id, name, description, price, quantity)
        VALUES (@Id, @Name, @Description, @Price, @Quantity)";

            NpgsqlCommand cmd2 = new NpgsqlCommand(insertQuery, conn);
            cmd2.Parameters.AddWithValue("@Id", id);
            cmd2.Parameters.AddWithValue("@Name", obj.name);
            cmd2.Parameters.AddWithValue("@Description", obj.description);
            cmd2.Parameters.AddWithValue("@Price", obj.price);
            cmd2.Parameters.AddWithValue("@Quantity", obj.quantity);

            cmd2.ExecuteNonQuery();

            return Ok(new
            {
                Message = "Data received successfully",
                obj
            });
        }



        [HttpDelete]
        [Route("Deletebyid/{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            using var conn = new NpgsqlConnection(
                _configuration.GetConnectionString("CrudAppconn"));

            string query = "DELETE FROM crud_data WHERE id = @Id";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            conn.Open();
            cmd.ExecuteNonQuery();

            return Ok(new
            {
                Message = "Data deleted successfully"
            });
        }



        [HttpPut]
        [Route("Updatelist/{id}")]
        public IActionResult Update([FromBody] crud obj, [FromRoute] int id)
        {
            using var conn = new NpgsqlConnection(
                _configuration.GetConnectionString("CrudAppconn"));

            string query = @"UPDATE crud_data 
                     SET name=@Name,
                         description=@Description,
                         price=@Price,
                         quantity=@Quantity
                     WHERE id=@Id";

            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Name", obj.name);
            cmd.Parameters.AddWithValue("@Description", obj.description);
            cmd.Parameters.AddWithValue("@Price", obj.price);
            cmd.Parameters.AddWithValue("@Quantity", obj.quantity);

            conn.Open();
            cmd.ExecuteNonQuery();

            return Ok(new
            {
                Message = "Data updated successfully"
            });
        }





        //sql server

        //[HttpGet]
        //[Route("GetAllLists")]
        //public string GetList()
        //{
        //    SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("CrudAppconn").ToString());
        //    SqlDataAdapter da = new SqlDataAdapter("select * from crud_data", conn);
        //    DataTable dt = new DataTable();
        //    da.Fill(dt);
        //    List<crud> crudlist = new List<crud>();
        //    Response response = new Response();
        //    if (dt.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {

        //            crud _crud = new crud();
        //            _crud.id = Convert.ToInt32(dt.Rows[i]["id"]);
        //            _crud.name = Convert.ToString(dt.Rows[i]["name"]);
        //            _crud.description = Convert.ToString(dt.Rows[i]["description"]);
        //            _crud.price = Convert.ToDouble(dt.Rows[i]["price"]);
        //            _crud.quantity = Convert.ToInt32(dt.Rows[i]["quantity"]);
        //            crudlist.Add(_crud);
        //        }
        //    }
        //    if (crudlist.Count > 0) {
        //        return JsonConvert.SerializeObject(crudlist);
        //    }
        //    else
        //    {
        //        response.StatusCode = 100;
        //        response.ErrorMessage = "No record Found";
        //        return JsonConvert.SerializeObject(response);
        //    }
        //}


        //[HttpGet]
        //[Route("GetById/{id}")]
        //public IActionResult getbyid([FromRoute] int id)
        //{


        //    SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("CrudAppconn").ToString());
        //    string query = "select * from crud_data where id =@Id";
        //    SqlDataAdapter dt = new SqlDataAdapter(query, conn);
        //    dt.SelectCommand.Parameters.AddWithValue("@Id", id);
        //    DataTable da = new DataTable();
        //    dt.Fill(da);
        //    crud crud = new crud();

        //    crud.id = Convert.ToInt32(da.Rows[0]["id"]);
        //    crud.name = Convert.ToString(da.Rows[0]["name"]);
        //    crud.description = Convert.ToString(da.Rows[0]["description"]);
        //    crud.price = Convert.ToInt32(da.Rows[0]["price"]);
        //    crud.quantity = Convert.ToInt32(da.Rows[0]["quantity"]);
        //    return Ok(crud);


        //}



        //[HttpPost]
        //[Route("PostList")]
        //public IActionResult postlist([FromBody] crud obj)
        //{
        //    if (obj == null)
        //    {
        //        return BadRequest("No data received");
        //    }
        //    SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("CrudAppconn").ToString());

        //    SqlCommand cmd = new SqlCommand("SELECT MAX(id) AS ID FROM crud_data", conn);
        //    conn.Open();
        //    obj.id = Convert.ToInt32(cmd.ExecuteScalar());
        //    int id = obj.id + 1;
        //    string query = "INSERT INTO crud_data (Id,Name,description, Price, Quantity) VALUES (@Id,@Name,@Description, @Price, @Quantity)";
        //    SqlCommand cmd2 = new SqlCommand(query, conn);
        //    cmd2.Parameters.AddWithValue("@id", id);
        //    cmd2.Parameters.AddWithValue("@Name", obj.name);
        //    cmd2.Parameters.AddWithValue("@Description", obj.description);
        //    cmd2.Parameters.AddWithValue("@Price", obj.price);
        //    cmd2.Parameters.AddWithValue("@Quantity", obj.quantity);
        //    cmd2.ExecuteNonQuery();
        //    //return Ok("Data Received");

        //    return Ok(new
        //    {
        //        Message = "Data received successfully",
        //        obj
        //    });

        //}



        //[HttpDelete]
        //[Route("Deletebyid/{id}")]
        //public IActionResult delete([FromRoute] int id)
        //{
        //    //if (DeleteList == null)
        //    //{
        //    //    return BadRequest("no data to delete");
        //    //}
        //    SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("CrudAppconn").ToString());
        //    string query = "delete from crud_data where id=@Id";
        //    SqlCommand cmd = new SqlCommand(query, conn);
        //    cmd.Parameters.AddWithValue("@Id", id);
        //    conn.Open();
        //    cmd.ExecuteNonQuery();
        //    return Ok(new
        //    {
        //        Message = "Data delete successfully"
        //    });
        //}


        //another approach

        //[HttpDelete("{id}")]
        //public IActionResult DeleteItem(int id)
        //{
        //SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("CrudAppconn").ToString());
        //string query = "delete from crud_data where id=@Id";
        //SqlCommand cmd = new SqlCommand(query, conn);
        //cmd.Parameters.AddWithValue("@Id", id);
        //conn.Open();
        //cmd.ExecuteNonQuery();

        //return NoContent(); // 204
        //}


        //[HttpPut]
        //[Route("Updatelist/{id}")]
        
        //public IActionResult Update([FromBody] crud obj,[FromRoute] int id)
        //{

        //    SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("CrudAppconn").ToString());
        //    string query = "update crud_data set name=@Name ,description=@Description,price=@Price,quantity=@Quantity where id=@Id";
        //    SqlCommand cmd = new SqlCommand(query, conn);
        //    cmd.Parameters.AddWithValue("@Id",id);
        //    cmd.Parameters.AddWithValue("@Name", obj.name);
        //    cmd.Parameters.AddWithValue("@Description", obj.description);
        //    cmd.Parameters.AddWithValue("@Price", obj.price);
        //    cmd.Parameters.AddWithValue("@Quantity", obj.quantity);
        //    conn.Open();
        //    cmd.ExecuteNonQuery();


        //    return Ok(new {
        //        Message="Data Update Successfully"
        //    });
        //}

       


    }
}
