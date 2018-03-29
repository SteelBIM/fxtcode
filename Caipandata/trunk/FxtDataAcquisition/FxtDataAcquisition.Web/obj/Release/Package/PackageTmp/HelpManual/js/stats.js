(function ($, Cookies) {
  function a(s) {
    return s.replace(/^http[s]?\:\/\/[^\/]*\/discovery/i, '');
  }
  var url = a(location.href);
  // from web
  var access_token = Cookies.get('access_token');
  var token_type = Cookies.get('token_type');
  // from app
  var token = Cookies.get('token');
  var _token = token ? ("Bearer " + token) : (access_token ? (token_type + " " + access_token) : 'Bearer 4m6OUvx5qcCZjxUw4iZ921pH7LvNZ3sWieUD3JEQ');

  var action = 'help';


  $.ajax({
    beforeSend: function(xhr){
        xhr.withCredentials = true;
    },
    xhrFields: { withCredentials: true },
    type: 'POST',
    url: 'https://api.huoban.com/v1/log',
    headers: {
        "Authorization": _token
    },
    contentType: 'application/json',
    dataType: 'json',
    data: '{"action": "' + action + '", "data": {"object": "' + url + '"}}',
    processData: false
  });

})(jQuery, Cookies);

