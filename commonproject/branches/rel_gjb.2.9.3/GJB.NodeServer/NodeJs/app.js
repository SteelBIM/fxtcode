var fs = require('fs')
    , http = require('http')
    , path = require('path')
    , io = require('socket.io')
    , express = require('express')
    , token = "Fxt_ABCDEFGHIJKLMNOPQRSTUVWXYZ"
    , users = []
    , undefined
    , socketio;

var app = express();
app.configure(function () {
    app.use(express.compress()); //启用gzip压缩
    app.use(express.static(path.join(__dirname, '/public')));//加载静态文件
    app.use(function (req, res) {
        res.writeHead(200, { 'Content-type': 'text/html' });
        res.end(fs.readFileSync(__dirname + '/index.html'));
    })
});

var server = http.createServer(app).listen(19898, function () {
    console.log('Listening at: http://192.168.0.7:19898');
});
socketio = io.listen(server);
socketio.on('connection', function (socket) {
    socket.on('sendtousers', function (data) {
        if (4 == data.type) {   //下线
            Commands.sendToLoggedInUser(data);
        }
        else {
            Commands.sendToUsers(data);
        }
    });
    socket.on('sendtodepartment', function (data) {
        Commands.sendToDepartment(data);
    });
    socket.on('sendtocompany', function (data) {
        Commands.sendToCompany(data);
    });
    socket.on('adduser', function (guest) {
        // we store the guest in the socket session for this client
        Commands.addUser(socket.id, guest);
    });
    socket.on('disconnect', function () {
        // remove the user from global users list
        Commands.leave(socket.id);
    });
});

// The list of interface commands the server can send / receive
var Commands = {
    addUser: function (socketid, guest) {
        var user = {};
        user.socketid = socketid;
        user.companyid = guest.companyid;
        user.departmentid = guest.departmentid;
        user.userid = guest.userid;
        user.username = guest.username
        users.push(user);
        console.log('connection: ', user);
    },
    getUser: function (companyid, departmentid, userid, username) {
        var loop = 0, result = [];
        for (; loop < users.length; loop++) {
            if (typeof (companyid) != "undefined" && typeof (departmentid) != "undefined" && typeof (userid) == "undefined" && typeof (username) == "undefined") {//部门
                if (users[loop].companyid == companyid && users[loop].departmentid == departmentid) {
                    result.push(users[loop]);
                }
            }
            else if (typeof (companyid) != "undefined" && typeof (departmentid) == "undefined" && typeof (userid) != "undefined" && typeof (username) == "undefined") {//用户Id
                if (users[loop].companyid == companyid && users[loop].userid == userid) {
                    result.push(users[loop]);
                }
            }
            else if (typeof (companyid) != "undefined" && typeof (departmentid) == "undefined" && typeof (username) != "undefined" && typeof (userid) == "undefined") {//用户账号                
                if (users[loop].companyid == companyid && users[loop].username == username) {
                    result.push(users[loop]);
                }
            }
            else if (typeof (companyid) != "undefined" && typeof (departmentid) == "undefined" && typeof (userid) == "undefined" && typeof (username) == "undefined") {//机构
                if (users[loop].companyid == companyid) {
                    result.push(users[loop]);
                }
            }
            else
                break;
        }
        return result;
    },
    leave: function (socketid) {
        var loop = 0;
        for (; loop < users.length; loop++) {
            if (users[loop].socketid == socketid) {
                console.log('disconnect:', users[loop]);
                users[loop].socketid = "";
                users[loop].companyid = "";
                users[loop].departmentid = "";
                users[loop].userid = "";
                users[loop].username = "";
                //delete users[loop];
                break;
            }
        }
    },
    sendToUsers: function (data) {
        if (typeof (data.token) != "undefined" && data.token == token) {
            console.log('sendToUsers Message : ', data);
            data.token = "";
            if (data.touserids) {
                var touserids = data.touserids.split(',');
                var companyid, userid, departmentid, users, i = 0;
                /* item格式: user_公司ID_用户ID */
                touserids.forEach(function (item) {
                    companyid = item.split('_')[1];
                    userid = item.split('_')[2];
                    users = Commands.getUser(companyid, departmentid, userid);
                    if (0 < users.length)//user is online
                    {
                        i = 0;
                        for (; i < users.length; i++) {
                            socketio.sockets.socket(users[i].socketid).emit('message', data);
                        }
                    }
                    else
                        console.log('user is not online: ', item);
                });
            }
            else if (data.tousernames) {
                var tousernames = data.tousernames.split(',');
                var companyid, username, departmentid, users, i = 0;
                /* item格式: user_公司ID_用户账号 */
                tousernames.forEach(function (item) {
                    companyid = item.split('_')[1];
                    username = item.split('_')[2];
                    users = Commands.getUser(companyid, departmentid, undefined, username);
                    if (0 < users.length)//user is online
                    {
                        i = 0;
                        for (; i < users.length; i++) {
                            socketio.sockets.socket(users[i].socketid).emit('message', data);
                        }
                    }
                    else {
                        console.log('user is not online: ', item);
                    }
                });

            }
            else
                console.log('touserids is null');
        }
        else
            console.log('token Error!', '');
    },
    sendToDepartment: function (data) {
        if (typeof (data.token) != "undefined" && data.token == token) {
            console.log('sendToDepartment Message : ', data);
            data.token = "";
            if (data.todepartmentids) {
                var todepartmentids = data.todepartmentids.split(',');
                var companyid, departmentid, users, i = 0;
                /* item格式: user_公司ID_部门ID */
                todepartmentids.forEach(function (item) {
                    companyid = item.split('_')[1];
                    departmentid = item.split('_')[2];
                    users = Commands.getUser(companyid, departmentid, undefined);
                    if (0 < users.length)//部门用户存在,在线的用户
                    {
                        i = 0;
                        for (; i < users.length; i++) {
                            socketio.sockets.socket(users[i].socketid).emit('message', data);
                        }
                    }
                    else
                        console.log('Department of the user is not online: ', item);
                });
            }
            else
                console.log('todepartmentids is null');
        }
        else
            console.log('token Error!', '');
    },
    sendToCompany: function (data) {
        if (typeof (data.token) != "undefined" && data.token == token) {
            console.log('sendToCompany Message : ', data);
            data.token = "";
            if (data.tocompanyids) {
                var tocompanyids = data.tocompanyids.split(',');
                var companyid, users, i = 0;
                /* item格式: user_公司ID */
                tocompanyids.forEach(function (item) {
                    companyid = item.split('_')[1];
                    users = Commands.getUser(companyid, undefined, undefined);
                    if (0 < users.length)//部门用户存在,在线的用户
                    {
                        i = 0;
                        for (; i < users.length; i++) {
                            socketio.sockets.socket(users[i].socketid).emit('message', data);
                        }
                    }
                    else
                        console.log('Company of the user is not online: ', item);
                });
            }
            else
                console.log('tocompanyids is null');
        }
        else
            console.log('token Error!', '');
    },
    sendToLoggedInUser: function (data) {
        if (typeof (data.token) != "undefined" && data.token == token) {
            console.log('sendToLoggedInUser Message : ', data);
            data.token = "";
            if (data.tousernames) {
                var tousernames = data.tousernames.split(',');
                var companyid, username, users, i = 0;
                /* item格式: user_公司ID_用户账号 */
                tousernames.forEach(function (item) {
                    companyid = item.split('_')[1];
                    username = item.split('_')[2];
                    users = Commands.getUser(companyid, undefined, undefined, username);
                    if (0 < users.length)//部门用户存在,在线的用户
                    {
                        i = 0;
                        var loggedinuser = [];
                        for (; i < users.length; i++) {
                            loggedinuser.push(users[i]);
                        }
                        i = 0;
                        for (; i < loggedinuser.length; i++) {
                            socketio.sockets.socket(loggedinuser[i].socketid).emit('message', data);
                            Commands.leave(loggedinuser[i].socketid);
                        }
                    }
                    else
                        console.log('Not logged in: ', item);
                });
            }
            else
                console.log('tousernames is null');
        }
        else
            console.log('token Error!', '');
    }
};