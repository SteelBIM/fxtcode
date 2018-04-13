// JavaScript Document
//初始化页面
$(function(){
	autoFixed();
	});

function autoFixed() {
    if (document.getElementById) {
        if (!window.opera) {
            if (window.document.body.offsetHeight) {
                var aH, tH, bH, trueH, maxH;
                aH = window.screen.availHeight;
                tH = window.document.getElementById("header").offsetHeight;
                if (window.document.getElementById("footer") != null) {
                    bH = window.document.getElementById("footer").offsetHeight;
                }
                if (window.document.getElementById("mainBody") != null) {
                    trueH = window.document.getElementById("mainBody").offsetHeight;
                }
                maxH = aH - tH - bH;
                if (maxH >= trueH) {
                    window.document.getElementById("footer").style.position = "fixed";
                }
            }
        }
    }
}
function autoFixedChild() {
    if (document.getElementById) {
        if (!window.opera) {
            if (window.parent.document.body.offsetHeight) {
                var aH, tH, bH, trueH, maxH;
                aH = window.parent.screen.availHeight;
                tH = window.parent.document.getElementById("header").offsetHeight;
                if (window.parent.document.getElementById("footer") != null) {
                    bH = window.parent.document.getElementById("footer").offsetHeight;
                }
                if (window.parent.document.getElementById("mainBody") != null) {
                    trueH = window.parent.document.getElementById("mainBody").offsetHeight;
                }
                maxH = aH - tH - bH;
                if (maxH >= trueH) {
                    window.parent.document.getElementById("footer").style.position = "fixed";
                }
            }
        }
    }
}

function autoMargin() {
    if (document.getElementById) {
        if (!window.opera) {
            if (window.document.body.offsetHeight) {
                var aH, tH, bH, trueH, maxH;
                aH = document.documentElement.clientWidth;
                tH = window.document.getElementById("description").offsetWidth;
                maxH = (aH - tH) / 2;
                window.document.getElementById("description").style.left = maxH + 'px';
            }
        }
    }
}