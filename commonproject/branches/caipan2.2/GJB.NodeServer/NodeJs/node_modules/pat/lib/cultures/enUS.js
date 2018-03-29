/**
 * Culture specific information.
 * @module pat
 * @submodule cultures
 */
/**
 * English language. US culture.
 * @static
 * @class cultures.enUS
 */

/*jslint white:true sloppy:true nomen:true*/

(function(root) {
    
    var culture = {
        
        id: 'enUS',
        
        /* Numbers */
        zeroDigit: '0',
        decimalSeparator: '.',
        groupingSeparator: ',',
        groupingSize: 3,
        
        /* Currency */
        currencySymbol: '$',
        currencyToken: 'USD',
        
        /* Weekday names */
        weekdays: ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'],
        weekdaysAbbr: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
        firstDayOfWeek: 1,
        
        /* Month names */
        months: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
        monthsAbbr: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
    
        /* Morning/afternoon tokens (e.g. 'am', 'pm') */
        amToken: 'am',
        pmToken: 'pm'
        
    };
    
    /* Node.js */
    if (typeof module !== 'undefined') {
        module.exports = culture;
    }
    /* AMD */
    else if (typeof define !== 'undefined' && define.amd) { //AMD
        define(culture);
    }
    /* Browser */
    else if (root) {
        if (!root.pat) { root.pat = {}; }
        if (!root.pat._private) { root.pat._private = {}; }
        if (!root.pat._private.cultures) {
            root.pat._private.cultures = {};
            root.pat._private.preferredCultureId = culture.id;
        }
        root.pat._private.cultures[culture.id] = culture;
    }
}(this));
