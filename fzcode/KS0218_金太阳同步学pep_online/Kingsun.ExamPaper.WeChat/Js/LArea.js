
window.LArea = (function() {
    var MobileArea = function() {
        this.gearArea;
        this.data;
        this.index = 0;
        this.value = [0];
    }
    MobileArea.prototype = {
        init: function(params) {
            this.params = params;
            this.trigger = document.querySelector(params.trigger);
            if(params.valueTo){
              this.valueTo=document.querySelector(params.valueTo);
            }
            this.keys = params.keys;
            this.type = params.type||1;
            switch (this.type) {
                case 1:
					break;
                default:
                    throw new Error('错误提示: 没有这种数据源类型');
                    break;
            }
            this.bindEvent();
        },
        getData: function(callback) {
            var _self = this;
            if (typeof _self.params.data == "object") {
                _self.data = _self.params.data;
                callback();
            } else {
                var xhr = new XMLHttpRequest();
                xhr.open('get', _self.params.data);
                xhr.onload = function(e) {
                    if ((xhr.status >= 200 && xhr.status < 300) || xhr.status === 0) {
                        var responseData = JSON.parse(xhr.responseText);
                        _self.data = responseData.data;
                        if (callback) {
                            callback()
                        };
                    }
                }
                xhr.send();
            }
        },
        bindEvent: function() {
            var _self = this;
			
            //呼出插件
            function popupArea(e) {
                _self.gearArea = document.createElement("div");
                _self.gearArea.className = "gearArea";
                _self.gearArea.innerHTML = '<div class="area_ctrl slideInUp">' +
                    '<div class="area_btn_box">' +
                    '<div class="area_btn larea_cancel">取消</div>' +
                    '<div class="area_btn larea_finish">完成</div>' +
                    '</div>' +
                    '<div class="area_roll_mask">' +
                    '<div class="area_roll">' +
                    '<div>' +
                    '<div class="gear area_province" data-areatype="area_province"></div>' +
                    '<div class="area_grid">' +
                    '</div>' +
                    '</div>' +
                   
                    '</div>' +
                    '</div>' +
                    '</div>';
                document.body.appendChild(_self.gearArea);
                areaCtrlInit();
                var larea_cancel = _self.gearArea.querySelector(".larea_cancel");
                larea_cancel.addEventListener('touchstart', function(e) {
                    _self.close(e);
                });
                var larea_finish = _self.gearArea.querySelector(".larea_finish");
                larea_finish.addEventListener('touchstart', function(e) {
                    _self.finish(e);
                });
                var area_province = _self.gearArea.querySelector(".area_province");
                
                area_province.addEventListener('touchstart', gearTouchStart);
               
                area_province.addEventListener('touchmove', gearTouchMove);
                
                area_province.addEventListener('touchend', gearTouchEnd);
                
            }
            //初始化插件默认值
            function areaCtrlInit() {
                _self.gearArea.querySelector(".area_province").setAttribute("val", _self.value[0]);
               
                switch (_self.type) {
                   case 1:
                        _self.setGearTooth(_self.data[0]);
                        break;
                }
            }
            //触摸开始
            function gearTouchStart(e) {
                e.preventDefault();
                var target = e.target;
                while (true) {
                    if (!target.classList.contains("gear")) {
                        target = target.parentElement;
                    } else {
                        break;
                    }
                }
                clearInterval(target["int_" + target.id]);
                target["old_" + target.id] = e.targetTouches[0].screenY;
                target["o_t_" + target.id] = (new Date()).getTime();
                var top = target.getAttribute('top');
                if (top) {
                    target["o_d_" + target.id] = parseFloat(top.replace(/em/g, ""));
                } else {
                    target["o_d_" + target.id] = 0;
                }
                target.style.webkitTransitionDuration = target.style.transitionDuration = '0ms';             
            }
            //手指移动
            function gearTouchMove(e) {
                e.preventDefault();
                var target = e.target;
                while (true) {
                    if (!target.classList.contains("gear")) {
                        target = target.parentElement;
                    } else {
                        break;
                    }
                }
                target["new_" + target.id] = e.targetTouches[0].screenY;
                target["n_t_" + target.id] = (new Date()).getTime();
                var f = (target["new_" + target.id] - target["old_" + target.id]) * 30 / window.innerHeight;
                target["pos_" + target.id] = target["o_d_" + target.id] + f;
                target.style["-webkit-transform"] = 'translate3d(0,' + target["pos_" + target.id] + 'em,0)';
                target.setAttribute('top', target["pos_" + target.id] + 'em');
                if(e.targetTouches[0].screenY<1){
                    gearTouchEnd(e);
                };
				
            }
             //离开屏幕
            function gearTouchEnd(e) {
                e.preventDefault();
                var target = e.target;
                while (true) {
                    if (!target.classList.contains("gear")) {
                        target = target.parentElement;
                    } else {
                        break;
                    }
                }
                var flag = (target["new_" + target.id] - target["old_" + target.id]) / (target["n_t_" + target.id] - target["o_t_" + target.id]);
                if (Math.abs(flag) <= 0.2) {
                    target["spd_" + target.id] = (flag < 0 ? -0.08 : 0.08);
                } else {
                    if (Math.abs(flag) <= 0.5) {
                        target["spd_" + target.id] = (flag < 0 ? -0.16 : 0.16);
                    } else {
                        target["spd_" + target.id] = flag / 2;
                    }
                }
                if (!target["pos_" + target.id]) {
                    target["pos_" + target.id] = 0;
                }
                rollGear(target);
            }
			
			
            //缓动效果
            function rollGear(target) {
                var d = 0;
                var stopGear = false;
                function setDuration() {
                    target.style.webkitTransitionDuration = target.style.transitionDuration = '200ms';
                    stopGear = true;
                }
                clearInterval(target["int_" + target.id]);
                target["int_" + target.id] = setInterval(function() {
                    var pos = target["pos_" + target.id];
                    var speed = target["spd_" + target.id] * Math.exp(-0.03 * d);
                    pos += speed;
                    if (Math.abs(speed) > 0.1) {} else {
                        var b = Math.round(pos / 2) * 2;
                        pos = b;
                        setDuration();
                    }
					
                    if (pos > 0) {
                        pos = 0;
                        setDuration();
                    }
					switch (_self.type) {
						case 1:
							 var minTop = -(eval(edition_data).length - 1) * 2;
							break;
                     }
                
                    
                    if (pos < minTop) {
                        pos = minTop;
                        setDuration();
                    }
					if (stopGear) {
                        var gearVal = Math.abs(pos) / 2;
                        setGear(target, gearVal);
                        clearInterval(target["int_" + target.id]);
                    }
                    target["pos_" + target.id] = pos;
                    target.style["-webkit-transform"] = 'translate3d(0,' + pos + 'em,0)';
                    target.setAttribute('top', pos + 'em');
                    d++;
                }, 30);
            }
             //控制插件滚动后停留的值
            function setGear(target, val) {
                val = Math.round(val);
                target.setAttribute("val", val);
                switch (_self.type) {
                    case 1:
                         _self.setGearTooth(_self.data[0]);
                        break;
                    
                }
                
            }
            _self.getData(function() {
                _self.trigger.addEventListener('click', popupArea);
            });
        },
        //重置节点个数
        setGearTooth: function(data) {
            var _self = this;
            var item = data || [];
            var l = item.length;
            var gearChild = _self.gearArea.querySelectorAll(".gear");
                 
            if (l > 0) {
                              
                var itemStr = "";
                for (var i = 0; i < l; i++) {
                    itemStr += "<div class='tooth'  ref='" + item[i][this.keys['id']] + "'>" + item[i][this.keys['name']] + "</div>";
                }
                gearChild[_self.index].innerHTML = itemStr;
          
            } 
        },
        finish: function(e) {
            var _self = this;
            var area_province = _self.gearArea.querySelector(".area_province");
            
            var provinceVal = parseInt(area_province.getAttribute("val"));
            var provinceText = area_province.childNodes[provinceVal].textContent;
            var provinceCode = area_province.childNodes[provinceVal].getAttribute('ref');
            
             _self.trigger.value = provinceText ;
            _self.value = [provinceVal];
            if(this.valueTo){
                this.valueTo.value= provinceCode;
            }
            _self.close(e);
        },
        close: function(e) {
            e.preventDefault();
            var _self = this;
            var evt = new CustomEvent('input');
            _self.trigger.dispatchEvent(evt);
            document.body.removeChild(_self.gearArea);
            _self.gearArea=null;
        },
		
    }
    return MobileArea;
})()