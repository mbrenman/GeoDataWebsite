using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace WebApplication1
{
    public static class Globals
    {
        private const string connectionReferenceName = "aquaDevelopment";
        private static ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[connectionReferenceName];

        public static string conString = Globals.settings.ConnectionString;
        public const string tableName = "ADDR1"; //ex: "COMPOSITE_BASE_PIPE_PZ"
    }
}