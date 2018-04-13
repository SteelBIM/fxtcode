// 需求：根据设计图的比例去动态设置不同屏幕下面对应的font-size值
// 这段JS不要添加入口函数，并且引用的时候放到最前面
// ui的大小根据自己的需求去改

// 设计图的宽度
var ui = 750;
// 自己设定的font值
var font = 24;
// 得到比例值
var ratio = ui/font;
var oHtml = document.documentElement;
var screenWidth = oHtml.offsetWidth;
// 初始的时候调用一次
getSize();

window.addEventListener('resize', getSize);
// 在resize的时候动态设置fontsize值
function getSize(){
  screenWidth = oHtml.offsetWidth;
  // 限制区间
  if(screenWidth <= 320){
    screenWidth = 320;
  }else if(screenWidth >= ui){
    screenWidth = ui;
  }
  oHtml.style.fontSize = screenWidth/ratio + 'px';
}