var LoadMask = function () {
    var Current = this;
    this.Show = function () {
        if ($("#maskdiv").length == 0) {
            Current.Inithtml();
        }
        Current.loadsize();
        $("#maskdiv").show();
        Current.bindScroll();
    }
    this.Hide = function () {
        $("#maskdiv").hide();
    }
    this.Inithtml = function () {
        var html = '<div id="maskdiv" style="display:none;"><div id="loading" class="loading">Loading pages...</div> </div> ';
        $(html).appendTo(document.body);
        $("#maskdiv").css("height", $(document).height());
        $("#maskdiv").css("width", $(document).width());
    }

    this.loadsize = function () {
        var top = (document.body.clientHeight - $("#loading").height()) / 2;
        var left = ($(window).width() - $("#loading").width()) / 2;
        var scrollTop = $(document).scrollTop();
        var scrollLeft = $(document).scrollLeft();
        $("#loading").css({ position: 'absolute', 'top': top + scrollTop, left: left + scrollLeft });
    }

    this.bindScroll = function () {
        $(window).unbind('scroll').bind('scroll', function () {
            Current.loadsize();
        });
    }
}