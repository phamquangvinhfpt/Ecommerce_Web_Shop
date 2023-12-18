const Cookie = {
    get: function (name) {
        name = name + "=";
        let ca = document.cookie.split(';');
        for (let i = 0; i < ca.length; i++) {
            let c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return null;
    },
    set: function (cname, cvalue, exdays) {
        const d = new Date();
        d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
        let expires = "expires=" + d.toUTCString();
        document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
    },
    remove: function (cname) {
        const d = new Date();
        d.setTime(d.getTime() - 100000);
        let expires = "expires=" + d.toUTCString();
        document.cookie = cname + "=;" + expires + ";path=/";
    },
    countProduct: function() {
        let count = 0;
        let ca = document.cookie.split(';');
        console.log(ca);
        for (let i = 0; i < ca.length; i++) {
          let c = ca[i];
          while (c.charAt(0) == ' ') {
            c = c.substring(1);
          }
          if (c.indexOf('product_') === 0) {
            let regex = /(?<=_)\d+(?==\d+)/g;
            let match = regex.exec(c);
            if (match) {
              count ++;
            }
          }
        }
        return count;
      },
    countWithPrefix: function (prefix) {
        let count = 0;
        let ca = document.cookie.split(';');
        for (let i = 0; i < ca.length; i++) {
            let c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(prefix) == 0) {
                count ++;
            }
        }
        return count;
    }
    
};

