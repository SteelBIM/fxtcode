// 配置 API 接口地址
var root = 'https://cnodejs.org/api/v1'

// 引用 axios
var axios = require('axios')

// 自定义判断元素类型 JS
function toType(obj) {
  return ({}).toString.call(obj).match(/\s([a-zA-Z]+)/)[1].toLowerCase()
}

// 参数过滤函数
function filterNull(o) {
  for(var key in o) {
    if(o[key] === null) {
      delete o[key]
    }
    if(toType(o[key]) === 'string') {
      o[key] = o[key].trim()
    }else if(toType(o[key]) === 'object') {
      o[key] = filterNull(o[key])
    }else if(toType(o[key]) === 'array') {
      o[key] = filterNull(o[key])
    }
  }
  return o
}

/*
* 接口处理函数
* 这个函数每个项目都是不一样的，我现在调整的是适用于 https://cnodejs.org/api/v1的接口
* 如果是其他接口，需要根据接口的参数进行调整
* 参考说明文档 https://cnodejs.org/topic/5378720ed6e2d16149fa16bd
* 主要是，不同的接口的成功标识和失败提示是不一致的，另外，不同的项目的处理方法也是不一致的，这里出错就是简单的 alert
*/
function apiAxios(method, url, params, success, failure) {
  if(params) {
    params = filterNull(params)
  }
  axios({
    url: url,
    method: method,
    data: method === 'POST' ||  method === 'GET' ? params : null,
    params: method === 'GET' ||  method === 'DELETE' ? params : null,
    baseURL: root,
    withCredentials: false
  })
  .then(function (res) {
    if(res.data.success === true) {
      if(success) {
        success(res.data)
      }
    }else {
      if(failure) {
        failure(res.data)
      }else {
        window.alert('error: ' + JSON.stringify(res.data))
      }
    }
  })
  .catch(function (err) {
    let res = err.response
    if(err) {
      window.alert('api error, HTTP CODE: ' + res.status)
    }
  })
}

// 返回在 vue 模板中的调用接口
export default {
  get: function (url, params, success, failure) {
    return apiAxios('GET', url, params, success, failure)
  },
  post: function (url, params, success, failure) {
    return apiAxios('POST', url, params, success, failure)
  },
  put: function (url, params, success, failure) {
    return apiAxios('PUT', url, params, success, failure)
  },
  delete: function (url, params, success, failure) {
    return apiAxios('DELETE', url, params, success, failure)
  }
}
