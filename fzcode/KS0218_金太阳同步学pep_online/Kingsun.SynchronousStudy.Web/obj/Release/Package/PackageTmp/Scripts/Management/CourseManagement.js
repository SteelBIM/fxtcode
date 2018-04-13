var Kingsun = Kingsun || {}
Kingsun.AppLibrary = Kingsun.AppLibrary || {}
Kingsun.AppLibrary.CourseManagement = Kingsun.AppLibrary.CourseManagement || {}
Kingsun.AppLibrary.CourseManagement = function () {
    this.AddCourse = function (AppID, CourseName, SubjectID, SubjectName, EditionID, EditionName, GradeID, GradeName, TermID, TermName, Version, ImageURL, Creator, Description, filePath, fileMD5, FirstPageNum, StageID, StageName, TryUpdate, Sort, Event) {
        var data = {
            AppID: AppID,
            CourseName: CourseName,
            SubjectID: SubjectID,
            SubjectName: SubjectName,
            EditionID: EditionID,
            EditionName: EditionName,
            GradeID: GradeID,
            GradeName: GradeName,
            TermID: TermID,
            TermName: TermName,
            Version: Version,
            ImageURL: ImageURL,
            Creator: Creator,
            Description: Description,
            FilePath: filePath,
            FileMD5: fileMD5,
            FirstPageNum: FirstPageNum,
            StageID: StageID,
            StageName: StageName,
            TryUpdate: TryUpdate,
            Sort: Sort
        };
        return Common.Ajax("CourseImplement", "AddCourse", data, Event);
    };

    this.EditCourse = function (ID, AppID, CourseName, SubjectID, SubjectName, EditionID, EditionName, GradeID, GradeName, TermID, TermName, Version, ImageURL, Creator, Description, StageID, StageName, Sort, Event) {
        var data = {
            ID: ID,
            AppID: AppID,
            CourseName: CourseName,
            SubjectID: SubjectID,
            SubjectName: SubjectName,
            EditionID: EditionID,
            EditionName: EditionName,
            GradeID: GradeID,
            GradeName: GradeName,
            TermID: TermID,
            TermName: TermName,
            Version: Version,
            ImageURL: ImageURL,
            Creator: Creator,
            Description: Description,
            StageID: StageID,
            StageName: StageName,
            Sort: Sort
        };
        return Common.Ajax("CourseImplement", "EditCourse", data, Event);
    };
    this.ActiveCourse = function (ID, Event) {
        var data = {
            ID: ID
        };
        return Common.Ajax("CourseImplement", "ActiveCourse", data, Event);
    };
    this.DisableCourse = function (ID, Event) {
        var data = {
            ID: ID
        };
        return Common.Ajax("CourseImplement", "DisableCourse", data, Event);
    };
    this.SelectCourse = function (ID, Event) {
        var data = {
            ID: ID
        };
        return Common.Ajax("CourseImplement", "SelectCourse", data, Event);
    };

    this.QueryCourse = function (WhereCondition, CurrentPageIndex, PageSize, Event) {
        var data = {
            Where: WhereCondition,
            PageIndex: CurrentPageIndex,
            PageSize: PageSize
        };
        return Common.Ajax("CourseImplement", "QueryCourse", data, Event);
    };
    this.QueryCourseName = function (appID, WhereCondition, CurrentPageIndex, PageSize, Event) {
        var data = {
            appID: appID,
            Where: WhereCondition,
            PageIndex: CurrentPageIndex,
            PageSize: PageSize
        };
        return Common.Ajax("CourseImplement", "QueryCourseName", data, Event);
    };

    this.UpdateCourse = function (CourseID, Version, UpdateUrl, Description, UpdateMD5, FirstPageNum, TryUpdate, CompleteURL, CompleteMD5, ModuleId, Event) {
        var data = {
            CourseID: CourseID,
            Version: Version,
            UpdateURL: UpdateUrl,
            Description: Description,
            UpdateMD5: UpdateMD5,
            FirstPageNum: FirstPageNum,
            TryUpdate: TryUpdate,
            CompleteURL: CompleteURL,
            CompleteMD5: CompleteMD5,
            ModuleID: ModuleId
        };
        return Common.Ajax("CourseImplement", "UpdateCourse", data, Event);
    };
    this.EditCourseVersion = function (ID, UpdateUrl, Description, UpdateMD5, FirstPageNum, TryUpdate, CompleteURL, CompleteMD5, ModuleId, Event) {
        var data = {
            ID: ID,
            UpdateURL: UpdateUrl,
            Description: Description,
            UpdateMD5: UpdateMD5,
            FirstPageNum: FirstPageNum,
            TryUpdate: TryUpdate,
            CompleteURL: CompleteURL,
            CompleteMD5: CompleteMD5,
            ModuleID: ModuleId
        };
        return Common.Ajax("CourseImplement", "EditCourseVersion", data, Event);
    };

    this.QueryCourseVersion = function (WhereCondition, CurrentPageIndex, PageSize, Event) {
        var data = {
            Where: WhereCondition,
            PageIndex: CurrentPageIndex,
            PageSize: PageSize
        };
        return Common.Ajax("CourseImplement", "QueryCourseVersion", data, Event);
    };

    this.ActiveCourseVersion = function (ID, CourseID, Event) {
        var data = {
            ID: ID,
            CourseID: CourseID
        };
        return Common.Ajax("CourseImplement", "ActiveCourseVersion", data, Event);
    };
    this.DisableCourseVersion = function (ID, CourseID, Event) {
        var data = {
            ID: ID,
            CourseID: CourseID
        };
        return Common.Ajax("CourseImplement", "DisableCourseVersion", data, Event);
    };
    this.SelectTopVersion = function (CourseID, Event) {
        var data = {
            CourseID: CourseID
        };
        return Common.Ajax("CourseImplement", "SelectTopVersion", data, Event);
    };
    this.SelectFirstVersion = function (ID, Event) {
        var data = {
            ID: ID
        };
        return Common.Ajax("CourseImplement", "SelectFirstVersion", data, Event);
    };
    this.Delete = function (ID, Event) {
        var data = {
            ID: ID
        };
        return Common.Ajax("CourseImplement", "DeleteCourse", data, Event);
    };
    this.SelectMaxDisableVersion = function (CourseID, Event) {
        var data = {
            CourseID: CourseID
        };
        return Common.Ajax("CourseImplement", "SelectMaxDisableVersion", data, Event);
    };

}
var courseManage = new Kingsun.AppLibrary.CourseManagement();