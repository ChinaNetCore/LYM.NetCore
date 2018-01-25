/*
 ** 函数：backwardsDisabled
 ** 功能：禁止浏览器返回
 ** 使用注意: 页面加载完成后使用
 */
function backwardsDisabled() {
    if (window.history && window.history.pushState) {
        $(window).on('popstate', function () {
            window.history.pushState('forward', null, '');
            window.history.forward(1);
        });
    }
    window.history.pushState('forward', null, '');
    window.history.forward(1);
}

/*
 ** 函数：parseDomain
 ** 功能：判断是否被其他页面嵌套
 ** 参数：domains ,有效访问的顶级域名，可为String或Array。
 ** 参数：href ,判断出不是有效访问域名后跳转的地址，可为String。
 ** 使用注意: 页面加载完成后使用
 */
var parseDomain = function (domains, href) {

    var href = href || "";
    if (!domains) return;
    var allowed = false;
    try {
        if (domains instanceof Array) {
            for (var i = 0; i < domains.length; i++) {
                if (window.top.document.domain === domains[i]) allowed = true;
            }
        } else if (typeof (domains) === "string") {
          
            if (window.top.document.domain === domains) allowed = true;


        }
    } catch (err) {
        console.log(err);
    }


    allowed == true ? "" : window.top.location.href = href;

};

/*
 ** 函数：isLowVersion
 ** 功能：判断浏览器版本是否小于ie9
 ** 使用注意: 页面加载完成后使用
 */

function isLowVersion() {
    var theUA = window.navigator.userAgent.toLowerCase();
    if ((theUA.match(/msie\s\d+/) && theUA.match(/msie\s\d+/)[0]) || (theUA.match(/trident\s?\d+/) && theUA.match(/trident\s?\d+/)[0])) {
        var ieVersion = theUA.match(/msie\s\d+/)[0].match(/\d+/)[0] || theUA.match(/trident\s?\d+/)[0];
        if (ieVersion < 9) {
            if (window.navigator.userAgent.indexOf('compatible') != -1) {
                alert('请将浏览器切换为极速模式');
            }

            var str = "555，你的浏览器版本太低了\n已经和时代脱轨了！";
            var str2 = "推荐使用:<a href='https://www.baidu.com/s?ie=UTF-8&wd=%E8%B0%B7%E6%AD%8C%E6%B5%8F%E8%A7%88%E5%99%A8' target='_blank' style='color:blue'>谷歌</a>," +
                "<a href='https://www.baidu.com/s?ie=UTF-8&wd=%E7%81%AB%E7%8B%90%E6%B5%8F%E8%A7%88%E5%99%A8' target='_blank' style='color:blue'>火狐</a>," +
                "<a href='https://www.baidu.com/s?ie=UTF-8&wd=360%e6%b5%8f%e8%a7%88%e5%99%a8' target='_blank' style='color:blue'>360浏览器</a>,其他双核急速模式";
            document.writeln("<pre style='text-align:center;color:#fff;background-color:#3c9; height:100%;border:0;position:fixed;top:0;left:0;width:100%;z-index:1234'>" +
                "<h2 style='padding-top:200px;margin:0'><strong>" + str + "<br/></strong></h2><p>" +
                str2 + "</p><h2 style='margin:0'><strong>如果你的使用的是双核浏览器,请切换到<a  target='_blank'  style='color:blue' href='https://www.baidu.com/s?ie=UTF-8&wd=%e6%b5%8f%e8%a7%88%e5%99%a8%e6%9e%81%e9%80%9f%e6%a8%a1%e5%bc%8f'>极速模式</a>访问<br/></strong></h2></pre>");
            document.execCommand("Stop");
        };
    }
}

window.onload=function(){
    isLowVersion();
    //parseDomain("http://www.cdstjyypt.com", "http://www.cdstjyypt.com:81");
}
  
   
