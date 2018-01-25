        $.fn.zTree._z.view.makeNodeIcoStyle = function(setting, node) {
            var icoStyle = [];
            if (!node.isAjaxing) {
                var icon = (node.isParent && node.iconOpen && node.iconClose) ? (node.open ? node.iconOpen : node.iconClose) : node[setting.data.key.icon];
                if (icon) icoStyle.push("background:url(", icon, ") 0 0 no-repeat;");
                if (setting.view.showIcon == false || (!$.fn.zTree._z.tools.apply(setting.view.showIcon), [setting.treeId, node], true)) {
                    icoStyle.push("display:none;"); //此处为更改处
                }
            }
            return icoStyle.join('');
        };

    