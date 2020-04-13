using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SlackOverload.Models
{
    public class DAL
    {
        private SqlConnection conn;

        public DAL(string connectionString)
        {
            conn = new SqlConnection(connectionString);
        }

        public int CreateQuestion(Question q)
        {
            q.Posted = DateTime.Now;
            q.Status = 1; //always create status=1

            string addQuery = "INSERT INTO Questions (Username, Title, Detail, Posted, Category, Tags, Status) ";
            addQuery += "VALUES (@Username, @Title, @Detail, @Posted, @Category, @Tags, @Status)"; 

            return conn.Execute(addQuery, q);
        }

        internal int CreateAnswer(Answer ans)
        {
            ans.Posted = DateTime.Now;
            ans.Upvotes = 0;

            string addQuery = "Insert Into Answers (Username, Detail, QuestionId, Posted, Upvotes) ";
            addQuery += "values (@Username, @Detail, @QuestionId, @Posted, @Upvotes)";

            return conn.Execute(addQuery, ans);
        }


        public IEnumerable<Answer> GetAnswersByQuestionId(int id)
        {
            string queryString = "SELECT * FROM Answers WHERE QuestionId = @id";
            return conn.Query<Answer>(queryString, new { id = id });
        }

        internal Answer GetAnswerById(int id)
        {
            string queryString = "SELECT * FROM Answers WHERE Id = @id";
            return conn.QueryFirstOrDefault<Answer>(queryString, new { id = id });
        }

        public Question GetQuestionById(int id)
        {
            string queryString = "SELECT * FROM Questions WHERE Id = @id";
            return conn.QueryFirstOrDefault<Question>(queryString, new { id = id });
        }

        internal int DeleteAnswerByID(int id)
        {
            string queryString = "Delete from Answers where Id = @ID";
            return conn.Execute(queryString, new { ID = id });
        }

        public int DeleteQuestionByID(int id)
        {
            string queryString = "Delete From Answers where QuestionID = @ID ";
            queryString += "Delete from Questions where Id = @ID";
            return conn.Execute(queryString, new { ID = id });
        }

        
        public IEnumerable<Question> GetQuestionsMostRecent()
        {
            string queryString = "SELECT TOP 20 * FROM Questions ORDER BY Posted DESC";
            return conn.Query<Question>(queryString);
        }


        public Answer UpdateAnswer(Answer a)
        {
            string queryString = "UPDATE Answers SET ";
            queryString += "username = @username, ";
            queryString += "Detail = @detail ";
            queryString += "where Id = @Id";
            conn.Execute(queryString, new { username = a.Username, detail = a.Detail, Id = a.Id });

            return GetAnswerById(a.Id);
        }

        

        public int UpdateQuestion(Question q)
        {
            string queryString = "update Questions set ";
            queryString += "Title = @Title, ";
            queryString += "Detail = @Detail, ";
            queryString += "Category = @Category, ";
            queryString += "Tags = @Tags, ";
            queryString += "Status = @Status ";
            queryString += "where Id = @Id";

            return conn.Execute(queryString, q);
        }

        internal IEnumerable<Question> GetQuestionsBySearch(string search)
        {
            string queryString = "SELECT * FROM Questions ";
            queryString += "where title like @search OR detail like @Search ";
            queryString += "ORDER BY Posted DESC ";
            return conn.Query<Question>(queryString, new { Search = "%" + search + "%" });
        }
    }
}
