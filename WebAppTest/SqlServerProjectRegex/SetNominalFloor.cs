using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Text.RegularExpressions;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString SetNominalFloor(string floorNo)
    {
        if (Regex.IsMatch(floorNo, "[#-]$|[0-9]$|[a-z]$|[A-Z]$"))
        {
            floorNo = floorNo + "层";
        }
        return floorNo;
    }
};

