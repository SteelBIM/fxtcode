/**
 * Select2 Chinese translation
 */
(function ($) {
    "use strict";
    $.extend($.fn.select2.defaults, {
        formatNoMatches: function () { return "û���ҵ�ƥ����"; },
        formatInputTooShort: function (input, min) { var n = min - input.length; return "��������" + n + "���ַ�";},
        formatInputTooLong: function (input, max) { var n = input.length - max; return "��ɾ��" + n + "���ַ�";},
        formatSelectionTooBig: function (limit) { return "��ֻ��ѡ�����" + limit + "��"; },
        formatLoadMore: function (pageNumber) { return "���ؽ����..."; },
        formatSearching: function () { return "������..."; }
    });
})(jQuery);
