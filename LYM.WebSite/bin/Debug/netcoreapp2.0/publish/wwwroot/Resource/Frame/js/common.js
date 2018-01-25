! function ($) {

    /*
    **  
    **  功能：时间选择器
    **  参数：option，参见daterangepicker的option设置。
    **  参数：callback，回调函数
    **  eg ： $('input').daterangepickers({},function(){});
    */

    $.fn.extend({
        daterangepickers: function (options, callback) {
            var locale = {
                format: "YYYY-MM-DD HH:mm:ss",  //控件中from和to 显示的日期格式  
                separator: " 到 ",
                applyLabel: "确定",
                cancelLabel: "取消",
                clearLabel: '清空',
                fromLabel: "开始",
                toLabel: "结束",
                customRangeLabel: "自定义",
                weekLabel: "W",
                daysOfWeek: ["日", "一", "二", "三", "四", "五", "六"],
                monthNames: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月 "],
                firstDay: 1,
            }

            var ranges = {//自制快捷键
                '今天': [moment(), moment()],
                '昨天': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                '最近7天': [moment().subtract(6, 'days'), moment()],
                '最近30天': [moment().subtract(29, 'days'), moment()],
                '这个月': [moment().startOf('month'), moment().endOf('month')],
                '上个月': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
            }

            options = options || {};

            options.ranges = !options.singleDatePicker ?
                $.extend({}, options.ranges, ranges) : options.ranges;

            options.applyClass =  "btn-danger"

            options.locale = options.locale ?
                function (_locale, locale) {
                    $.each(locale, function (i, n) {
                        _locale[i] = _locale[i] || n;
                    });
                    return _locale;
                }(options.locale, locale)
                : locale;

            return $(this).daterangepicker(options, callback);
        }
    })


    /*
    **  
    **  功能：自适应字体大小
    **  参数：min，最小的font-size，单位像素。
    **  参数：max，最大的font-size，单位像素。
    **  参数：mid，范围缓冲区，设置在60-70之间效果最佳。值越小产生的字体越大，反之字体越小，可以根据实际情况来调整这个参数。
    **  eg ： $('body').fontFlex(14, 20, 70);
    */

    $.fn.fontFlex = function (min, max, mid) {
        var $this = this;

        $(window).resize(function () {

            var size = window.innerWidth / mid;

            if (size < min) size = min;
            if (size > max) size = max;

            $this.css('font-size', size + 'px');

        }).trigger('resize');
    };

    if ($.fn.iCheck) {
        $('input').iCheck({
            checkboxClass: 'icheckbox_flat-green',
            radioClass: 'iradio_flat-green',
            //increaseArea: '20%' // optional
        });
    }



}(jQuery);


//将form中的值转换为键值对。
function getFormJson(frm) {
    var o = {};
    var a = $(frm).serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });

    return o;
}




/* 
** 拖动滚动条
*/

(function (root, factory) {
    if (typeof define === 'function' && define.amd) {
        define(['exports'], factory);
    } else if (typeof exports !== 'undefined') {
        factory(exports);
    } else {
        factory((root.dragscroll = {}));
    }
}(this, function (exports) {
    var _window = window;
    var mousemove = 'mousemove';
    var mouseup = 'mouseup';
    var mousedown = 'mousedown';
    var addEventListener = 'addEventListener';
    var removeEventListener = 'removeEventListener';

    var dragged = [];
    var reset = function (i, el) {
        for (i = 0; i < dragged.length;) {
            el = dragged[i++];
            el[removeEventListener](mousedown, el.md, 0);
            _window[removeEventListener](mouseup, el.mu, 0);
            _window[removeEventListener](mousemove, el.mm, 0);
        }

        dragged = document.getElementsByClassName('dragscroll');
        for (i = 0; i < dragged.length;) {
            (function (el, lastClientX, lastClientY, pushed) {
                el[addEventListener](
                    mousedown,
                    el.md = function (e) {
                        pushed = 1;
                        lastClientX = e.clientX;
                        lastClientY = e.clientY;

                        e.preventDefault();
                        e.stopPropagation();
                    }, 0
                );

                _window[addEventListener](
                    mouseup, el.mu = function () { pushed = 0; }, 0
                );

                _window[addEventListener](
                    mousemove,
                    el.mm = function (e, scroller) {
                        scroller = el.scroller || el;
                        if (pushed) {
                            scroller.scrollLeft -=
                                (-lastClientX + (lastClientX = e.clientX));
                            scroller.scrollTop -=
                                (-lastClientY + (lastClientY = e.clientY));
                        }
                    }, 0
                );
            })(dragged[i++]);
        }
    }


    if (document.readyState === "complete") {
        reset();
    } else {
        _window[addEventListener]("load", reset, 0);
    }

    exports.reset = reset;
}));



/*
** 函数：checkAll(this)
** 功能：全选功能
*/

function checkAll(obj) {
    var parent = $(obj).parent();
    if (!parent.hasClass("select-muliple")) return;

    var siblings = parent.siblings();
    if (parent.hasClass("selected")) {
        siblings.removeClass("selected");
    } else {
        siblings.addClass("selected");
    }
    parent.toggleClass("selected");
}

/*
** 函数：checkSelf(obj)
** 功能：多选
** 参数：obj为this
** 例：onclick="checkSelf(this)"
*/

function checkSelf(obj) {
    var parent = $(obj).parent();
    parent.toggleClass("selected");

    if ($(".select-muliple").length !== 0) {//全选功能存在
        var size = $(".selected:not('.select-muliple')").length;
        var totle = parent.siblings("tr").length;
        var selectAll = $(".select-muliple");
        if (size === totle) {
            selectAll.addClass("selected");
        }
        else {
            selectAll.removeClass("selected");
        }
    } else {
        parent.siblings().removeClass("selected");
    }

}



/*
** 功能：表单验证
*/

if ($.validator) {
    $.validator.setDefaults({
        highlight: function (e) {
            $(e).closest(".display_flex").removeClass("has-success").addClass("has-error")
        },
        success: function (e, element) {
            //console.log(element)
            $(element).is(':radio') || $(element).is(':checkbox') ?
                e.html("").addClass("has-succeed") :
                e.html("<i class='fa fa-check' style='color:#3c9;' aria-hidden='true'></i>").addClass("has-success")

            e.nextAll(".tip-info").addClass("hide");
            // e.html("Ok!").addClass("has-secceed");

            //e.closest(".display_flex").removeClass("has-error").addClass("has-success")
        },
        errorElement: "span",
        errorPlacement: function (error, element) {
            console.log(error, element)
            error.children().hasClass("fa") ? "" :
                error.prepend("<i class='fa fa-times-circle'></i>&nbsp;");
            if (element.is(':radio') || element.is(':checkbox')) { //如果是radio或checkbox
                var eid = element.attr('name'); //获取元素的name属性

                element.parent().hasClass("icheckbox_flat-green") ||
                    element.parent().hasClass("iradio_flat-green") ?
                    error.appendTo(element.parent().parent()) :
                    error.appendTo(element.parent()); //将错误信息添加当前元素的父结点后面
            } else {
                error.insertAfter(element);
            }
            element.nextAll(".tip-info").addClass("hide")
        },
        errorClass: "validate-error",
        validClass: "has-success",

    });
}


/*
** 函数：reinitIframe(obj)
** 功能：重置iframe高度
** 参数：obj为this
** 例：onclick="reinitIframe(this)"
*/
function reinitIframe(obj) {
    var iframe = obj;
    try {
        var bHeight = iframe.contentWindow.document.body.scrollHeight;
        var dHeight = iframe.contentWindow.document.documentElement.scrollHeight;
        var height = Math.max(bHeight, dHeight);
        iframe.height = height + 20;
    } catch (ex) {
        console.log(ex)
    }
}

if (typeof (toastr) === "object") {
    toastr.options = {
        closeButton: true,
        debug: false,
        progressBar: false,
        positionClass: "toast-top-right",
        onclick: null,
        showDuration: "300",
        hideDuration: "1000",
        timeOut: "5000",
        extendedTimeOut: "1000",
        showEasing: "swing",
        hideEasing: "linear",
        showMethod: "fadeIn",
        hideMethod: "fadeOut"
    };
}

function getDataTableLocal() {
    var dataTableLocal = {
        "sProcessing": "处理中...",
        "sLengthMenu": "显示 _MENU_ 项结果",
        "sZeroRecords": "没有匹配结果",
        "sInfo": "显示第 _START_ 至 _END_ 项结果，共 _TOTAL_ 项",
        "sInfoEmpty": "显示第 0 至 0 项结果，共 0 项",
        "sInfoFiltered": "(由 _MAX_ 项结果过滤)",
        "sInfoPostFix": "",
        "sSearch": "搜索:",
        "sUrl": "",
        "sEmptyTable": "表中数据为空",
        "sLoadingRecords": "载入中...",
        "sInfoThousands": ",",
        "oPaginate": {
            "sFirst": "首页",
            "sPrevious": "上页",
            "sNext": "下页",
            "sLast": "末页"
        },
        "oAria": {
            "sSortAscending": ": 以升序排列此列",
            "sSortDescending": ": 以降序排列此列"
        }
    }
    return dataTableLocal;
}

function HandleData(data, csuccess, cffailed) {

    if (data.responseJSON) {
        data = data.responseJSON;
    }
    var t = window.toastr ? window.toastr : parent.toastr;

    var content;
    if (data.ResultType === 0) {
        
        if (data.Message) {
            content = data.Message;
        } else {
            content = '操作成功';
        }

        t.success(content, "提示", {
            'positionClass': "toast-center-center", "closeButton": true, "onHidden": function () {
                if (csuccess && typeof (csuccess) === 'function')
                    csuccess(data);
            }
        });
    } else {
        if (data.Message) {
            content = data.Message;
        } else {
            content = '操作失败';
        }

        t.error(content, "提示", {
            'positionClass': "toast-center-center", "closeButton": true, "onHidden": function () {
                if (cffailed && typeof (cffailed) === 'function')
                    cffailed(data);
            }
        });

    }


}

