using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Text.RegularExpressions;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString TestFunction(string input)
    {
        if (Regex.IsMatch(input, "[#-]$|[0-9]$|[a-z]$|[A-Z]$"))
        {
            input = input + "层";
        }
        return input;
    }
};

