(function () {

    if (typeof utils === 'undefined') { utils = {}; }

    Array.prototype.findItem = function (prop, value) {
        if (this == null || this === undefined)
            return undefined;
        for (var i = 0, len = this.length; i < len; i++)
            if (this[i][prop] == value) return this[i];
        return undefined;
    }

    Array.prototype.removeItem = function (item) {
        if (this == null || this === undefined)
            return;
        for (var i = 0; i < this.length; i++) {
            if (this[i] === item) {
                this.splice(i, 1);
                return;
            }
        }
    }
})();