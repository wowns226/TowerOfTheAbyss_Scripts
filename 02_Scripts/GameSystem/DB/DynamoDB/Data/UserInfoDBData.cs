// Copyright (C) <2023>  
//     Authors : Shin YongUk <dyddyddnr@naver.com>
//     Lim jaejun <wowns226@naver.com>
//  
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//  
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//  
//     You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>

using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

public class ChapterDBData
{
    public ChapterDBData(int chapter, int floor)
    {
        this.chapter = chapter;
        this.floor = floor;
    }

    public int chapter;
    public int floor;
}

[DynamoDBTable("USER_INFO")]
public class UserInfoDBData : IDBData
{
    [DynamoDBHashKey] // Hash key.
    public ulong id { get; set; }
    [DynamoDBProperty]
    public string achievement { get; set; }
    [DynamoDBProperty]
    public string nickname { get; set; }
    [DynamoDBProperty]
    public List<ChapterDBData> chapters { get; set; }
}
