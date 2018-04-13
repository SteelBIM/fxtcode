
accessid = '';
accesskey = '';
host = '';
policyBase64 = '';
signature = '';

callbackbody = ''
filename = ''
key = ''
expire = 0
g_object_name = ''
g_object_name_type = ''
now = timestamp = Date.parse(new Date()) / 1000;

function send_request() {
    var xmlhttp = null;
    if (window.XMLHttpRequest) {
        xmlhttp = new XMLHttpRequest();
    }
    else if (window.ActiveXObject) {
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    if (xmlhttp != null) {
        serverUrl = '/Module/GetPostObjectSignature'
        xmlhttp.open("GET", serverUrl, false);
        xmlhttp.send(null);
        return xmlhttp.responseText
    }
};

function check_object_radio() {//设置为随机生成的新的文件名
    //var tt = document.getElementsByName('myradio');
    //for (var i = 0; i < tt.length ; i++ )
    //{
    //    if(tt[i].checked)
    //    {
    //        g_object_name_type = tt[i].value;
    //        break;
    //    }
    //}
    g_object_name_type = 'random_name';
}

function get_signature() {
    now = timestamp = Date.parse(new Date()) / 1000;
    if (expire < now + 3) {
        body = send_request()
        var obj = eval("(" + body + ")");
        host = obj['host']
        accessid = obj['accessid']
        accesskey = obj['accesskey']
        policyBase64 = obj['policy']
        var bytes = Crypto.HMAC(Crypto.SHA1, policyBase64, accesskey, { asBytes: true });
        signature = Crypto.util.bytesToBase64(bytes);
        expire = parseInt(obj['expire'])
        key = obj['dir']
        return true;
    }
    return false;
};

function random_string(len) {
    var len = len || 32;
    var chars = 'ABCDEFGHJKMNPQRSTWXYZabcdefhijkmnprstwxyz2345678';
    var maxPos = chars.length;
    var pwd = '';
    for (i = 0; i < len; i++) {
        pwd += chars.charAt(Math.floor(Math.random() * maxPos));
    }
    return pwd;
}

function get_suffix(filename) {
    pos = filename.lastIndexOf('.')
    suffix = ''
    if (pos != -1) {
        suffix = filename.substring(pos)
    }
    return suffix;
}

function calculate_object_name(filename) {
    if (g_object_name_type == 'local_name') {
        g_object_name += "${filename}"
    }
    else if (g_object_name_type == 'random_name') {
        suffix = get_suffix(filename)
        g_object_name = key + random_string(10) + suffix
    }
    return ''
}

function get_uploaded_object_name(filename) {
    if (g_object_name_type == 'local_name') {
        tmp_name = g_object_name
        tmp_name = tmp_name.replace("${filename}", filename);
        return tmp_name
    }
    else if (g_object_name_type == 'random_name') {
        return g_object_name
    }
}

function set_upload_param(up, filename, ret) {
    if (ret == false) {
        ret = get_signature()
    }
    g_object_name = key;
    if (filename != '') {
        suffix = get_suffix(filename)
        calculate_object_name(filename)
    }
    new_multipart_params = {
        'key': g_object_name,
        'policy': policyBase64,
        'OSSAccessKeyId': accessid,
        'success_action_status': '200', 
        'signature': signature,
    };
    up.setOption({
        'url': host,
        'multipart_params': new_multipart_params
    });
    up.start();
}

var uploader = new plupload.Uploader({
    runtimes: 'html5,flash',
    browse_button: 'selectfiles',
    container: document.getElementById('container'),
    flash_swf_url: '/Scripts/aliyunupload/plupload.flash.swf',
    url: 'http://oss.aliyuncs.com',
    filters: {
        mime_types: [
        //{ title: "Mp4 files", extensions: "mp4" }
        ],
        max_file_size: '1000mb',
        prevent_duplicates: true
    },
    init: {
        PostInit: function () {
            document.getElementById('ossfile').innerHTML = '';
            document.getElementById('postfiles').onclick = function () {
                set_upload_param(uploader, '', false);
                return false;
            };
        },
        FilesAdded: function (up, files) {//已添加文件
            plupload.each(files, function (file) {
                document.getElementById('ossfile').innerHTML += '<div id="' + file.id + '">' + file.name + ' (' + plupload.formatSize(file.size) + ')<b></b>'
				+ '<div class="progress"><div class="progress-bar" style="width: 0%"></div></div>'
				+ '</div>';
            });
        },

        BeforeUpload: function (up, file) {//上传前
            check_object_radio();
            set_upload_param(up, file.name, true);
        },

        UploadProgress: function (up, file) {//上传进度
            var d = document.getElementById(file.id);
            d.getElementsByTagName('b')[0].innerHTML = '<span>' + file.percent + "%</span>";
            var prog = d.getElementsByTagName('div')[0];
            var progBar = prog.getElementsByTagName('div')[0]
            progBar.style.width = 2 * file.percent + 'px';
            progBar.setAttribute('aria-valuenow', file.percent);
        },

        FileUploaded: function (up, file, info) {//文件已上传成功
            //$("#lessonsize").text((file.size / 1024 / 1024).toFixed(1));//文件大小
            $("#MarketBookCatalogCover").val("http://tbxcdn.kingsun.cn/" + get_uploaded_object_name(file.name));//文件名加播放地址
            if (info.status == 200) {
                document.getElementById(file.id).getElementsByTagName('b')[0].innerHTML = '<br />上传成功。';
            }
            else {
                document.getElementById(file.id).getElementsByTagName('b')[0].innerHTML = info.response;
            }
        },
        Error: function (up, err) {
            //{"code":-200,"message":"HTTP Error.","file":{"id":"o_1br5q83ej1opdthd1n1h18fr1m857","name":"初级01.mp4","type":"video/mp4","size":25450457,"origSize":25450457,"loaded":0,"percent":0,"status":4,"lastModifiedDate":"2017-09-01T09:10:50.399Z"},"response":"","status":0,"responseHeaders":""}
            if (err.code == -600) {
                document.getElementById('console').appendChild(document.createTextNode(""));
            }
            else if (err.code == -601) {
                document.getElementById('console').appendChild(document.createTextNode(""));
            }
            else if (err.code == -602) {
                document.getElementById('console').appendChild(document.createTextNode(""));
            }
            else {
                document.getElementById('console').appendChild(document.createTextNode("\nError xml:" + err.response));
            }
        }

    }

});

uploader.init();
