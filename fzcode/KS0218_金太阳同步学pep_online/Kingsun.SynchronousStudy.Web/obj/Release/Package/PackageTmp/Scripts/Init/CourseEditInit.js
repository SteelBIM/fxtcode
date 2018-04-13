/// <reference path="../jquery.json-2.4.js" />
/// <reference path="../Common.js" />
/// <reference path="../Management/CourseManagement.js" />
/// <reference path="../Management/ServiceManagement.js" />

var CourseEidtInit = function () {
    var Current = this;
    this.CID = undefined;
    this.AppID = undefined;
    this.Init = function () {
        Current.CID = Common.QueryString.GetValue("CourseID");
        Current.AppID = Constant.AppID;
        if (Current.AppID == undefined || Current.AppID == null || Current.AppID == "") {
            /////////如果无法确定当前应用信息，则返回上一个页面
            alert("无法确定当前应用ID！");
            window.history.go(-1);
            return;
        } else {
            $("#hidden_appID").val(Current.AppID);
        }

        ////////////////////////////////////////////////////////////
        ////////加载Select
        ///////////////////////////////////////////////////////////



        Common.CodeData.InitData("SSTAGE,GRADE,SUB,ED,BREEL", function () {

            var stageData = [];
            var stage = Common.CodeData.GetData("SSTAGE");
            for (var i in stage) {
                stageData.push({ id: stage[i].ID, text: stage[i].CodeName, selected: (!parseInt(i)), desc: "" });
            }
            $('#select_stage').combobox('loadData', stageData);

            var gradeData = [];
            var grade = Common.CodeData.GetData("GRADE");
            for (var i in grade) {
                gradeData.push({ id: grade[i].ID, text: grade[i].CodeName, selected: (!parseInt(i)), desc: "" });
            }
            $('#select_grade').combobox('loadData', gradeData);

            var subjectData = [];
            var subject = Common.CodeData.GetData("SUB");
            for (var i in subject) {
                subjectData.push({ id: subject[i].ID, text: subject[i].CodeName, selected: (!parseInt(i)), desc: "" });
            }
            $('#select_subject').combobox('loadData', subjectData);


            var editionData = [];
            var edition = Common.CodeData.GetData("ED");
            for (var i in edition) {
                editionData.push({ id: edition[i].ID, text: edition[i].CodeName, selected: (!parseInt(i)), desc: "" });
            }
            $('#select_edition').combobox('loadData', editionData);

            var termData = [];
            var term = Common.CodeData.GetData("BREEL");
            for (var i in term) {
                termData.push({ id: term[i].ID, text: term[i].CodeName, selected: (!parseInt(i)), desc: "" });
            }
            $('#select_term').combobox('loadData', termData);

        });

        //id = 0,
        //            text = "—请选择年级—",
        //            selected = true,
        //            desc = ""

        //////////////////////////////////////////////////////////


        ///////////////////////////////////////////////////////////
        /////////初始化
        if (Current.CID != null && Current.CID != "") {
            $("#hidden_courseID").val(Current.CID);
            courseManage.SelectCourse(Current.CID, function (data) {
                if (data.Success) {
                    Current.SetValues(data.Data);
                } else {
                    alert("加载课程信息失败！");
                }
                $("#select_stage").combobox("disable");
                $("#select_edition").combobox("disable");
                $("#select_grade").combobox("disable");
                $("#select_subject").combobox("disable");
                $("#select_term").combobox("disable");
                $("#input_version").attr("readonly", "readonly");
                $("#tr_filePath").hide();
                $("#tr_fileMD5").hide();
                $("#tryupdatetr").hide();
                //$("#FirstPageNum").attr("disabled", true);
            });
        } else {
            $("#input_createTime").val(new Date().format("yyyy.MM.dd hh:mm"));
            //$("#input_creator").val("测试用户");
            $("#input_creator").val(Common.Cookie.GetUserName());
            $("#input_version").val("1.0.0");
        }
        ///////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////
        //////事件注册
        $("#bt_save").click(function () {
            //保存
            var obj = Current.GetValues();
            if (obj == null) {
                ////信息不完整，取消保存操作
                return;
            }
            if (obj.ID == undefined || obj.ID == "") {
                /////////////////添加
                var filePath = $("#input_file").val();
                var FirstPageNum;
                //var FirstPageNum = $("#FirstPageNum").val();
                //课程文件地址不为空
                if (filePath == undefined || filePath == "") {
                    alert("请输入课程文件!");
                    return null;
                }
                if (!Common.Validate.IsURL(filePath)) {
                    alert("课程文件不是有效的URL地址!");
                    return null;
                }
                if (filePath.indexOf(".zip") <= -1) {
                    alert("更新文件必须是zip格式!");
                    return null;
                }

                var errMsg = Common.ValidateTxt(filePath);
                if (errMsg != "" && errMsg != undefined) {
                    alert(errMsg);
                    $("#input_file").focus();
                    return null;
                }
                var fileMD5 = $("#input_fileMD5").val();

                if (fileMD5 == undefined || fileMD5 == "") {
                    alert("请输入课程文件MD5值!");
                    return null;
                }

                serviceManage.QueryCourseExists(Current.AppID, obj.EditionID, obj.GradeID, obj.SubjectID, obj.TermID, obj.StageID, function (data) {
                    if (data.Success && !data.Data) {
                        //////请求成功并且该课程不存在，则允许添加
                        courseManage.AddCourse(Current.AppID, obj.CourseName, obj.SubjectID, obj.SubjectName, obj.EditionID, obj.EditionName, obj.GradeID,
                            obj.GradeName, obj.TermID, obj.TermName, obj.Version, obj.ImageURL, obj.Creator, obj.Description, filePath, fileMD5, FirstPageNum, obj.StageID, obj.StageName, obj.TryUpdate, obj.Sort, function (data) {
                                if (data.Success) {
                                    alert("课程添加成功！");
                                    window.location = "CourseManager.aspx?appID=" + Current.AppID;
                                } else {
                                    alert("课程添加失败！提示：" + data.ErrorMsg);
                                }
                            });
                    } else {
                        alert("课程已经存在！");
                        return;
                    }
                });

            } else {
                ///修改
                courseManage.EditCourse(obj.ID, Current.AppID, obj.CourseName, obj.SubjectID, obj.SubjectName, obj.EditionID, obj.EditionName, obj.GradeID,
                obj.GradeName, obj.TermID, obj.TermName, obj.Version, obj.ImageURL, obj.Creator, obj.Description, obj.StageID, obj.StageName, obj.Sort, function (data) {
                    if (data.Success) {
                        alert("课程修改成功！");
                        window.location = "CourseManager.aspx?appID=" + Current.AppID;
                    } else {
                        alert("课程修改失败！提示：" + data.ErrorMsg);
                    }
                });
            }
        });
        $("#bt_cancel").click(function () {
            //取消
            window.history.go(-1);
        });
        ///////////////////////////////////////////////////////////
    };
    this.SetValues = function (data) {
        $("#hidden_filePath").val(data.ImageURL);
        $('#img_pic').attr("src", "../" + data.ImageURL);
        $("#input_version").val(data.Version.replace("V", "").replace("v", ""));
        $("#input_createTime").val(Common.FormatTime(data.CreateDateTime, "yyyy.MM.dd hh:mm"));
        $("#input_creator").val(data.Creator);
        $("#sort").val(data.Sort);
        $("#text_desc").val(data.Description);
        $("#select_edition").combobox('setValue', data.EditionID);
        $("#select_grade").combobox('setValue', data.GradeID);
        $("#select_subject").combobox('setValue', data.SubjectID);
        $("#select_term").combobox('setValue', data.TermID);
        $("#select_stage").combobox('setValue', data.StageID);
        //设置首页页码的值
        //var course = courseManage.SelectMaxDisableVersion(Current.CID);
        //if (course.Success) {
        //    $("#FirstPageNum").val(course.Data.FirstPageNum);
        //}
    };
    this.GetValues = function () {
        var id = $("#hidden_courseID").val();
        var picPath = $("#hidden_filePath").val();
        if (picPath == undefined || picPath == "") {
            alert("请上传课程图片!");
            return null;
        }
        var stage = $('#select_stage').combobox('getValue');
        if (stage == undefined || stage == 0) {
            alert("请选择学段!");
            return null;
        }

        var subject = $('#select_subject').combobox('getValue');
        if (subject == undefined || subject == 0) {
            alert("请选择学科!");
            return null;
        }
        var edition = $('#select_edition').combobox('getValue');
        if (edition == undefined || edition == 0) {
            alert("请选择版本!");
            return null;
        }
        var grade = $('#select_grade').combobox('getValue');
        if (grade == undefined || grade == 0) {
            alert("请选择年级!");
            return null;
        }
        var term = $('#select_term').combobox('getValue');
        if (term == undefined || term == 0) {
            alert("请选择学期!");
            return null;
        }

        var version = $("#input_version").val();
        if (version == undefined || version == "") {
            alert("请输入课程版本!");
            return null;
        }
        //        if (!Common.Validate.IsVersion(version)) {
        //            alert("请正确输入课程版本!");
        //            return null;
        //        }
        version = "v" + version;

        var createTime = $("#input_createTime").val();
        var creator = $("#input_creator").val();

        var desc = $("#text_desc").val();
        if (desc == undefined || desc == "") {
            alert("请上传课程描述!");
            return null;
        }
        if (desc.length > 150) {
            alert("课程描述不能超过150字!");
            return null;
        }
        //desc = Common.HtmlEncode(desc);

        var errMsg = Common.ValidateTxt(desc);
        if (errMsg != "" && errMsg != undefined) {
            alert(errMsg);
            $("#text_desc").focus();
            return null;
        }


        //课程首页页码
        //var firstPageNum = $("#FirstPageNum").val();
        //if (firstPageNum != undefined) {
        //    if (firstPageNum > 20) {
        //        alert("首页页码不能大于20！");
        //        return null;
        //    }
        //}
        //else {
        //    alert("首页页码不能为空！");
        //    return null;
        //}
        var stageName = $('#select_stage').combobox('getText');
        var courseName = stageName + $('#select_edition').combobox('getText') + $('#select_subject').combobox('getText')
                       + $('#select_grade').combobox('getText') + $('#select_term').combobox('getText');
        var subjectText = $('#select_subject').combobox('getText');
        var editionText = $('#select_edition').combobox('getText');
        var gradeText = $('#select_grade').combobox('getText');
        var termText = $('#select_term').combobox('getText');
        var sort = $("#sort").val();
        if (isNaN(sort)) {
            alert("排序值必须是数字");
        }
        var obj = {
            ID: id,
            CourseName: courseName,
            SubjectID: subject,
            SubjectName: subjectText,
            EditionID: edition,
            EditionName: editionText,
            GradeID: grade,
            GradeName: gradeText,
            TermID: term,
            TermName: termText,
            Version: version,
            ImageURL: picPath,
            Creator: creator,
            Description: desc,
            StageID: stage,
            StageName: stageName,
            TryUpdate: 0,
            Sort: sort
        };

        return obj;
    };

}