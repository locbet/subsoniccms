//the following code from: http://www.kryogenix.org/code/browser/sorttable/sorttable.js
//modified by: Dave Neeley
//modified date: 2007

//addEvent(window, "load", sortables_init);

//var SORT_COLUMN_INDEX;

//function sortables_init() {
//    // Find all tables with class sortable and make them sortable
//    if (!document.getElementsByTagName) return;
//    tbls = document.getElementsByTagName("table");
//    for (ti=0;ti<tbls.length;ti++) {
//        thisTbl = tbls[ti];
//        if (((' '+thisTbl.className+' ').indexOf("sortable") != -1) && (thisTbl.id)) {
//            //initTable(thisTbl.id);
//            ts_makeSortable(thisTbl);
//        }
//    }
//}

//function ts_makeSortable(table) {
//    if (table.rows && table.rows.length > 0) {
//        var firstRow = table.rows[0];
//    }
//    if (!firstRow) return;
//    
//    // We have a first row: assume it's the header, and make its contents clickable links
//    for (var i=0;i<firstRow.cells.length;i++) {
//        var cell = firstRow.cells[i];
//        if (firstRow.cells[i].innerHTML != '&nbsp;' && (firstRow.cells[i].innerHTML.search('onclick') == -1))
//        {
//           var txt = ts_getInnerText(cell);
//           cell.innerHTML = '<a href="#" onclick="ts_resortTable(this);return false;">'+txt+'<img class="sortarrow" src="../../../images/icons/arrow_none.gif"></img></a>';
//        }
//    }
//}

//function ts_getInnerText(el) {
//	if (typeof el == "string") return el;
//	if (typeof el == "undefined") { return el };
//   if (!el.innerHTML && el.innerText) return el.innerText;
//	var str = ts_getInnerTextforCellType(el);
//	if (str == "")
//	{
//	   var cs = el.childNodes;
//	   var l = cs.length;
//	   for (var i = 0; i < l; i++) {
//		   switch (cs[i].nodeType) {
//			   case 1: //ELEMENT_NODE
//				   str += ts_getInnerText(cs[i]);
//				   break;
//			   case 3:	//TEXT_NODE
//			      str += cs[i].nodeValue;
//				   break;
//		   }
//	   }
//	}
//	return str;
//}

//function ts_getInnerTextforCellType(el){
//   var str = "";
//   var quit = 0;
//	for (iLoop=0; iLoop<el.getElementsByTagName("option").length; iLoop++)
//   {
//      
//      browser = navigator.appName;
//      
//      //detect if Internet Explorer
//      if (browser.search(/microsoft/i) == 0)
//      {
//         if (el.getElementsByTagName("option").item(iLoop).getAttribute("selected") == true)
//         {
//            str = el.getElementsByTagName("option").item(iLoop).getAttribute("value");
//            quit = 1;
//            break;
//         }           
//      }
//      else
//      {
//      //other browser.  Well, expecting firefox
//         if (el.getElementsByTagName("option").item(iLoop).getAttribute("selected") == "selected")
//         {
//            str = el.getElementsByTagName("option").item(iLoop).getAttribute("value");
//            quit = 1;
//            break;
//         } 
//      }
//      
//      
//   }
//   if (quit == 0)
//   {
//      if (el.getElementsByTagName("input").length > 0);
//      {
//	      for (iLoop=0; iLoop<el.getElementsByTagName("input").length; iLoop++)
//         {
//            var value = el.getElementsByTagName("input").item(iLoop).getAttribute("type");
//            switch(value)
//            {
//               case "radio":
//               if (el.getElementsByTagName("option").item(iLoop).getAttribute("checked") != 0)
//               {
//                  str = el.getElementsByTagName("input").item(iLoop).getAttribute("value").value;
//               }
//               break;
//               
//               case "checkbox":
//                  str = '0'
//                  var on = el.getElementsByTagName("input").item(iLoop).getAttribute("value");
//                  if (el.getElementsByTagName("input").item(iLoop).getAttribute("checked") == true) { str = '1'};
//               break;

//               default:
//                  str = el.getElementsByTagName("input").item(iLoop).getAttribute("value");
//               break;
//            }
//         }
//      }
//   }
//   return str
//}

//function ts_resortTable(lnk) {
//    // get the span
//    var span;
//    for (var ci=0;ci<lnk.childNodes.length;ci++) {
//        if (lnk.childNodes[ci].tagName && lnk.childNodes[ci].tagName.toLowerCase() == 'img') span = lnk.childNodes[ci];
//    }
//    var spantext = ts_getInnerText(span);
//    var td = lnk.parentNode;
//    var column = td.cellIndex;
//    var table = getParent(td,'TABLE');
//    
//    // Work out a type for the column
//    if (table.rows.length <= 1) return;
//    var itm = ts_getInnerText(table.rows[1].cells[column]);
//    sortfn = ts_sort_caseinsensitive;
//    if (itm.match(/^\d\d[\/-]\d\d[\/-]\d\d\d\d$/)) sortfn = ts_sort_date;
//    if (itm.match(/^\d\d[\/-]\d\d[\/-]\d\d$/)) sortfn = ts_sort_date;
//    if (itm.match(/^[£$]/)) sortfn = ts_sort_currency;
//    if (itm.match(/^[\d\.]+$/)) sortfn = ts_sort_numeric;
//    SORT_COLUMN_INDEX = column;
//    var firstRow = new Array();
//    var newRows = new Array();
//    for (i=0;i<table.rows[0].length;i++) { firstRow[i] = table.rows[0][i]; }
//    for (j=1;j<table.rows.length;j++) { newRows[j-1] = table.rows[j]; }

//    newRows.sort(sortfn);

//    if (span.getAttribute("sortdir") == 'down') {
//        ARROW = '../../../images/icons/arrow_down.gif'
//        newRows.reverse();
//        span.setAttribute('sortdir','up');
//    } else {
//        ARROW = '../../../images/icons/arrow_up.gif'
//        span.setAttribute('sortdir','down');
//    }
//    
//    // We appendChild rows that already exist to the tbody, so it moves them rather than creating new ones
//    // don't do sortbottom rows
//    for (i=0;i<newRows.length;i++) { if (!newRows[i].className || (newRows[i].className && (newRows[i].className.indexOf('sortbottom') == -1))) table.tBodies[0].appendChild(newRows[i]);}
//    // do sortbottom rows only
//    for (i=0;i<newRows.length;i++) { if (newRows[i].className && (newRows[i].className.indexOf('sortbottom') != -1)) table.tBodies[0].appendChild(newRows[i]);}
// 
//    // Re-color rows - known limitation in IE (it doesn't work there)
//    for (i=0;i<newRows.length;i++) 
//    {
//      if (i % 2 != 0) 
//      { 
//         newRows[i].setAttribute('class','GridCell2');
//      } 
//      else 
//      { 
//         newRows[i].setAttribute('class','GridCell1');
//      }
//   }
//    
//    // Delete any other arrows there may be showing
//    var allspans = document.getElementsByTagName("img");
//    for (var ci=0;ci<allspans.length;ci++) {
//        if (allspans[ci].className == 'sortarrow') {
//            if (getParent(allspans[ci],"table") == getParent(lnk,"table")) { // in the same table as us?
//                allspans[ci].setAttribute('src','../../../images/icons/arrow_none.gif');
//            }
//        }
//    }
//        
//    span.setAttribute('src',ARROW)

//}

//function getParent(el, pTagName) {
//	if (el == null) return null;
//	else if (el.nodeType == 1 && el.tagName.toLowerCase() == pTagName.toLowerCase())	// Gecko bug, supposed to be uppercase
//		return el;
//	else
//		return getParent(el.parentNode, pTagName);
//}
//function ts_sort_date(a,b) {
//    // y2k notes: two digit years less than 50 are treated as 20XX, greater than 50 are treated as 19XX
//    aa = ts_getInnerText(a.cells[SORT_COLUMN_INDEX]);
//    bb = ts_getInnerText(b.cells[SORT_COLUMN_INDEX]);
//    if (aa.length == 10) {
//        dt1 = aa.substr(6,4)+aa.substr(3,2)+aa.substr(0,2);
//    } else {
//        yr = aa.substr(6,2);
//        if (parseInt(yr) < 50) { yr = '20'+yr; } else { yr = '19'+yr; }
//        dt1 = yr+aa.substr(3,2)+aa.substr(0,2);
//    }
//    if (bb.length == 10) {
//        dt2 = bb.substr(6,4)+bb.substr(3,2)+bb.substr(0,2);
//    } else {
//        yr = bb.substr(6,2);
//        if (parseInt(yr) < 50) { yr = '20'+yr; } else { yr = '19'+yr; }
//        dt2 = yr+bb.substr(3,2)+bb.substr(0,2);
//    }
//    if (dt1==dt2) return 0;
//    if (dt1<dt2) return -1;
//    return 1;
//}

//function ts_sort_currency(a,b) { 
//    aa = ts_getInnerText(a.cells[SORT_COLUMN_INDEX]).replace(/[^0-9.]/g,'');
//    bb = ts_getInnerText(b.cells[SORT_COLUMN_INDEX]).replace(/[^0-9.]/g,'');
//    return parseFloat(aa) - parseFloat(bb);
//}

//function ts_sort_numeric(a,b) { 
//    aa = parseFloat(ts_getInnerText(a.cells[SORT_COLUMN_INDEX]));
//    if (isNaN(aa)) aa = 0;
//    bb = parseFloat(ts_getInnerText(b.cells[SORT_COLUMN_INDEX])); 
//    if (isNaN(bb)) bb = 0;
//    return aa-bb;
//}

//function ts_sort_caseinsensitive(a,b) {
//    aa = ts_getInnerText(a.cells[SORT_COLUMN_INDEX]).toLowerCase();
//    bb = ts_getInnerText(b.cells[SORT_COLUMN_INDEX]).toLowerCase();
//    if (aa==bb) return 0;
//    if (aa<bb) return -1;
//    return 1;
//}

//function ts_sort_default(a,b) {
//    aa = ts_getInnerText(a.cells[SORT_COLUMN_INDEX]);
//    bb = ts_getInnerText(b.cells[SORT_COLUMN_INDEX]);
//    if (aa==bb) return 0;
//    if (aa<bb) return -1;
//    return 1;
//}

//function addEvent(elm, evType, fn, useCapture)
//// addEvent and removeEvent
//// cross-browser event handling for IE5+,  NS6 and Mozilla
//// By Scott Andrew
//{
//  if (elm.addEventListener){
//    elm.addEventListener(evType, fn, useCapture);
//    return true;
//  } else if (elm.attachEvent){
//    var r = elm.attachEvent("on"+evType, fn);
//    return r;
//  } else {
//    alert("Handler could not be removed");
//  }
//} 


//this is version 2
//at first glance it doesn't work !?
var stIsIE = /*@cc_on!@*/false;

sorttable = {
  init: function() {
    // quit if this function has already been called
    if (arguments.callee.done) return;
    // flag this function so we don't do the same thing twice
    arguments.callee.done = true;
    // kill the timer
    if (_timer) clearInterval(_timer);
    
    if (!document.createElement || !document.getElementsByTagName) return;
    
    sorttable.DATE_RE = /^(\d\d?)[\/\.-](\d\d?)[\/\.-]((\d\d)?\d\d)/;
    
    gridSort_forEach(document.getElementsByTagName('table'), function(table) {
      if (table.className.search(/\bsortable\b/) != -1) {
        sorttable.makeSortable(table);
      }
    });
    
  },
  
  makeSortable: function(table) {
    if (table.getElementsByTagName('thead').length == 0) {
      // table doesn't have a tHead. Since it should have, create one and
      // put the first table row in it.
      the = document.createElement('thead');
      the.appendChild(table.rows[0]);
      table.insertBefore(the,table.firstChild);
    }
    // Safari doesn't support table.tHead, sigh
    if (table.tHead == null) table.tHead = table.getElementsByTagName('thead')[0];
    
    if (table.tHead.rows.length != 1) return; // can't cope with two header rows
    
    // Sorttable v1 put rows with a class of "sortbottom" at the bottom (as
    // "total" rows, for example). This is B&R, since what you're supposed
    // to do is put them in a tfoot. So, if there are sortbottom rows,
    // for backwards compatibility, move them to tfoot (creating it if needed).
    sortbottomrows = [];
    for (var i=0; i<table.rows.length; i++) {
      if (table.rows[i].className.search(/\bsortbottom\b/) != -1) {
        sortbottomrows[sortbottomrows.length] = table.rows[i];
      }
    }
    if (sortbottomrows) {
      if (table.tFoot == null) {
        // table doesn't have a tfoot. Create one.
        tfo = document.createElement('tfoot');
        table.appendChild(tfo);
      }
      for (var i=0; i<sortbottomrows.length; i++) {
        tfo.appendChild(sortbottomrows[i]);
      }
      delete sortbottomrows;
    }
    
    // work through each column and calculate its type
    headrow = table.tHead.rows[0].cells;
    for (var i=0; i<headrow.length; i++) {
      // manually override the type with a sorttable_type attribute
      if (!headrow[i].className.match(/\bsorttable_nosort\b/)) { // skip this col
        mtch = headrow[i].className.match(/\bsorttable_([a-z0-9]+)\b/);
        if (mtch) { override = mtch[1]; }
	      if (mtch && typeof sorttable["sort_"+override] == 'function') {
	        headrow[i].sorttable_sortfunction = sorttable["sort_"+override];
	      } else {
	        headrow[i].sorttable_sortfunction = sorttable.guessType(table,i);
	      }
	      // make it clickable to sort
	      headrow[i].sorttable_columnindex = i;
	      headrow[i].sorttable_tbody = table.tBodies[0];
	      dean_addEvent(headrow[i],"mouseover", function(e) {this.style.cursor='hand';});
	      dean_addEvent(headrow[i],"click", function(e) {

          var childToRemove = null;
          if (this.className.search(/\bsorttable_sorted\b/) != -1) {
            // if we're already sorted by this column, just 
            // reverse the table, which is quicker
            sorttable.reverse(this.sorttable_tbody);
            this.className = this.className.replace('sorttable_sorted',
                                                    'sorttable_sorted_reverse');
            childToRemove = document.getElementById(findParentID(this,'sorttable_sortfwdind'));
            if (childToRemove != null && childToRemove.parentNode == this)
				this.removeChild(childToRemove);
            sortrevind = document.createElement('span');
            sortrevind.id = findParentID(this,'sorttable_sortrevind');
            sortrevind.innerHTML = '<span class="gridSort">&nbsp</span>';
            this.appendChild(sortrevind);
            return;
          }
          if (this.className.search(/\bsorttable_sorted_reverse\b/) != -1) {
            // if we're already sorted by this column in reverse, just 
            // re-reverse the table, which is quicker
            sorttable.reverse(this.sorttable_tbody);
            this.className = this.className.replace('sorttable_sorted_reverse',
                                                    'sorttable_sorted');
            childToRemove = document.getElementById(findParentID(this,'sorttable_sortrevind'));
            if (childToRemove != null && childToRemove.parentNode == this)
				this.removeChild(childToRemove);
            sortfwdind = document.createElement('span');
            sortfwdind.id = findParentID(this,'sorttable_sortfwdind');
            sortfwdind.innerHTML = '<span class="gridSortReverse">&nbsp</span>';
            this.appendChild(sortfwdind);
            return;
          }
          
          // remove sorttable_sorted classes
          theadrow = this.parentNode;
          gridSort_forEach(theadrow.childNodes, function(cell) {
            if (cell.nodeType == 1) { // an element
              cell.className = cell.className.replace('sorttable_sorted_reverse','');
              cell.className = cell.className.replace('sorttable_sorted','');
            }
          });
          sortfwdind = document.getElementById(findParentID(this,'sorttable_sortfwdind'));
          if (sortfwdind) { sortfwdind.parentNode.removeChild(sortfwdind); }
          sortrevind = document.getElementById(findParentID(this,'sorttable_sortrevind'));
          if (sortrevind) { sortrevind.parentNode.removeChild(sortrevind); }
          
          this.className += ' sorttable_sorted';
          sortfwdind = document.createElement('span');
          sortfwdind.id = findParentID(this,'sorttable_sortfwdind');
          sortfwdind.innerHTML = '<span class="gridSortReverse">&nbsp</span>';
          this.appendChild(sortfwdind);

	        // build an array to sort. This is a Schwartzian transform thing,
	        // i.e., we "decorate" each row with the actual sort key,
	        // sort based on the sort keys, and then put the rows back in order
	        // which is a lot faster because you only do getInnerText once per row
	        row_array = [];
	        col = this.sorttable_columnindex;
	        rows = this.sorttable_tbody.rows;
	        for (var j=0; j<rows.length; j++) {
	          row_array[row_array.length] = [sorttable.getInnerText(rows[j].cells[col]), rows[j]];
	        }
	        /* If you want a stable sort, uncomment the following line */
	        //sorttable.shaker_sort(row_array, this.sorttable_sortfunction);
	        /* and comment out this one */
	        row_array.sort(this.sorttable_sortfunction);
	        
	        tb = this.sorttable_tbody;
	        for (var j=0; j<row_array.length; j++) {
			  /* In case you were wondering, the recoloring doesn't work in IE6 */
			  if (j % 2 != 0) {
				 row_array[j][1].setAttribute('class','GridCell2');
				 }
			  else {
				 row_array[j][1].setAttribute('class','GridCell1');
				}
	          tb.appendChild(row_array[j][1]);
	        }

	        delete row_array;
	      });
	    }
    }
  },
  
  guessType: function(table, column) {
    // guess the type of a column based on its first non-blank row
    sortfn = sorttable.sort_alpha;
    for (var i=0; i<table.tBodies[0].rows.length; i++) {
      text = sorttable.getInnerText(table.tBodies[0].rows[i].cells[column]);
      if (text != '') {
        if (text.match(/^-?[£$¤]?[\d,.]+%?$/)) {
          return sorttable.sort_numeric;
        }
        // check for a date: dd/mm/yyyy or dd/mm/yy 
        // can have / or . or - as separator
        // can be mm/dd as well
        possdate = text.match(sorttable.DATE_RE)
        if (possdate) {
          // looks like a date
          first = parseInt(possdate[1]);
          second = parseInt(possdate[2]);
          if (first > 12) {
            // definitely dd/mm
            return sorttable.sort_ddmm;
          } else if (second > 12) {
            return sorttable.sort_mmdd;
          } else {
            // looks like a date, but we can't tell which, so assume
            // that it's dd/mm (English imperialism!) and keep looking
            sortfn = sorttable.sort_ddmm;
          }
        }
      }
    }
    return sortfn;
  },
  
  getInnerText: function(node) {
    // gets the text we want to use for sorting for a cell.
    // strips leading and trailing whitespace.
    // this is *not* a generic getInnerText function; it's special to sorttable.
    // for example, you can override the cell text with a customkey attribute.
    // it also gets .value for <input> fields.
    
    hasInputs = (typeof node.getElementsByTagName == 'function') &&
                 node.getElementsByTagName('input').length;
    
    if (node.getAttribute && node.getAttribute("sorttable_customkey") != null) {
      return node.getAttribute("sorttable_customkey");
    }
    else if (typeof node.textContent != 'undefined' && !hasInputs) {
      return node.textContent.replace(/^\s+|\s+$/g, '');
    }
    else if (typeof node.innerText != 'undefined' && !hasInputs) {
      return node.innerText.replace(/^\s+|\s+$/g, '');
    }
    else if (typeof node.text != 'undefined' && !hasInputs) {
      return node.text.replace(/^\s+|\s+$/g, '');
    }
    else {
      switch (node.nodeType) {
        case 3:
          if (node.nodeName.toLowerCase() == 'input') {
            return node.value.replace(/^\s+|\s+$/g, '');
          }
        case 4:
          return node.nodeValue.replace(/^\s+|\s+$/g, '');
          break;
        case 1:
        case 11:
          var innerText = '';
          for (var i = 0; i < node.childNodes.length; i++) {
            innerText += sorttable.getInnerText(node.childNodes[i]);
          }
          return innerText.replace(/^\s+|\s+$/g, '');
          break;
        default:
          return '';
      }
    }
  },
  
  reverse: function(tbody) {
    // reverse the rows in a tbody
    newrows = [];
    for (var i=0; i<tbody.rows.length; i++) {
      newrows[newrows.length] = tbody.rows[i];
    }
    for (var i=newrows.length-1; i>=0; i--) {
       tbody.appendChild(newrows[i]);
    }
    delete newrows;
  },
  
  /* sort functions
     each sort function takes two parameters, a and b
     you are comparing a[0] and b[0] */
  sort_numeric: function(a,b) {
    aa = parseFloat(a[0].replace(/[^0-9.-]/g,''));
    if (isNaN(aa)) aa = 0;
    bb = parseFloat(b[0].replace(/[^0-9.-]/g,'')); 
    if (isNaN(bb)) bb = 0;
    return aa-bb;
  },
  sort_alpha: function(a,b) {
    if (a[0]==b[0]) return 0;
    if (a[0]<b[0]) return -1;
    return 1;
  },
  sort_ddmm: function(a,b) {
    mtch = a[0].match(sorttable.DATE_RE);
    if (mtch != null){
    y = mtch[3]; m = mtch[2]; d = mtch[1];
    if (m.length == 1) m = '0'+m;
    if (d.length == 1) d = '0'+d;
    dt1 = y+m+d;} 
    else {
    dt1 = '19000101'; }
    mtch = b[0].match(sorttable.DATE_RE);
    if (mtch != null){
    y = mtch[3]; m = mtch[2]; d = mtch[1];
    if (m.length == 1) m = '0'+m;
    if (d.length == 1) d = '0'+d;
    dt2 = y+m+d;} 
    else {
    dt2 = '19000101'; }
    if (dt1==dt2) return 0;
    if (dt1<dt2) return -1;
    return 1;
  },
  sort_mmdd: function(a,b) {
    mtch = a[0].match(sorttable.DATE_RE);
    if (mtch != null){
    y = mtch[3]; d = mtch[2]; m = mtch[1];
    if (m.length == 1) m = '0'+m;
    if (d.length == 1) d = '0'+d;
    dt1 = y+m+d; } 
    else {
    dt1 = '19000101'; }
    mtch = b[0].match(sorttable.DATE_RE);
    if (mtch != null){
    y = mtch[3]; d = mtch[2]; m = mtch[1];
    if (m.length == 1) m = '0'+m;
    if (d.length == 1) d = '0'+d;
    dt2 = y+m+d; } 
    else {
    dt2 = '19000101'; }
    if (dt1==dt2) return 0;
    if (dt1<dt2) return -1;
    return 1;
  },
  
  shaker_sort: function(list, comp_func) {
    // A stable sort function to allow multi-level sorting of data
    // see: http://en.wikipedia.org/wiki/Cocktail_sort
    // thanks to Joseph Nahmias
    var b = 0;
    var t = list.length - 1;
    var swap = true;

    while(swap) {
        swap = false;
        for(var i = b; i < t; ++i) {
            if ( comp_func(list[i], list[i+1]) > 0 ) {
                var q = list[i]; list[i] = list[i+1]; list[i+1] = q;
                swap = true;
            }
        } // for
        t--;

        if (!swap) break;

        for(var i = t; i > b; --i) {
            if ( comp_func(list[i], list[i-1]) < 0 ) {
                var q = list[i]; list[i] = list[i-1]; list[i-1] = q;
                swap = true;
            }
        } // for
        b++;

    } // while(swap)
  }  
}


/* ******************************************************************
   Supporting functions: bundled here to avoid depending on a library
   ****************************************************************** */

// Dean Edwards/Matthias Miller/John Resig

/* for Mozilla/Opera9 */
if (document.addEventListener) {
    document.addEventListener("DOMContentLoaded", sorttable.init, false);
}

/* for Internet Explorer */
/*@cc_on @*/
/*@if (@_win32)
    document.write("<script id=__ie_onload defer src=javascript:void(0)><\/script>");
    var script = document.getElementById("__ie_onload");
    script.onreadystatechange = function() {
        if (this.readyState == "complete") {
            sorttable.init(); // call the onload handler
        }
    };
/*@end @*/

/* for Safari */
if (/WebKit/i.test(navigator.userAgent)) { // sniff
    var _timer = setInterval(function() {
        if (/loaded|complete/.test(document.readyState)) {
            sorttable.init(); // call the onload handler
        }
    }, 10);
}

/* for other browsers */
window.onload = sorttable.init;

// written by Dean Edwards, 2005
// with input from Tino Zijdel, Matthias Miller, Diego Perini

// http://dean.edwards.name/weblog/2005/10/add-event/

function dean_addEvent(element, type, handler) {
	if (element.addEventListener) {
		element.addEventListener(type, handler, false);
	} else {
		// assign each event handler a unique ID
		if (!handler.$$guid) handler.$$guid = dean_addEvent.guid++;
		// create a hash table of event types for the element
		if (!element.events) element.events = {};
		// create a hash table of event handlers for each element/event pair
		var handlers = element.events[type];
		if (!handlers) {
			handlers = element.events[type] = {};
			// store the existing event handler (if there is one)
			if (element["on" + type]) {
				handlers[0] = element["on" + type];
			}
		}
		// store the event handler in the hash table
		handlers[handler.$$guid] = handler;
		// assign a global event handler to do all the work
		element["on" + type] = handleEvent;
	}
};
// a counter used to create unique IDs
dean_addEvent.guid = 1;

function removeEvent(element, type, handler) {
	if (element.removeEventListener) {
		element.removeEventListener(type, handler, false);
	} else {
		// delete the event handler from the hash table
		if (element.events && element.events[type]) {
			delete element.events[type][handler.$$guid];
		}
	}
};

function handleEvent(event) {
	var returnValue = true;
	// grab the event object (IE uses a global event object)
	event = event || fixEvent(((this.ownerDocument || this.document || this).parentWindow || window).event);
	// get a reference to the hash table of event handlers
	var handlers = this.events[event.type];
	// execute each event handler
	for (var i in handlers) {
		this.$$handleEvent = handlers[i];
		if (this.$$handleEvent(event) === false) {
			returnValue = false;
		}
	}
	return returnValue;
};

function fixEvent(event) {
	// add W3C standard event methods
	event.preventDefault = fixEvent.preventDefault;
	event.stopPropagation = fixEvent.stopPropagation;
	return event;
};
fixEvent.preventDefault = function() {
	this.returnValue = false;
};
fixEvent.stopPropagation = function() {
  this.cancelBubble = true;
}

// Dean's gridSort_forEach: http://dean.edwards.name/base/gridSort_forEach.js
/*
	gridSort_forEach, version 1.0
	Copyright 2006, Dean Edwards
	License: http://www.opensource.org/licenses/mit-license.php
*/

// array-like enumeration
if (!Array.gridSort_forEach) { // mozilla already supports this
	Array.gridSort_forEach = function(array, block, context) {
		for (var i = 0; i < array.length; i++) {
			block.call(context, array[i], i, array);
		}
	};
}

// generic enumeration
Function.prototype.gridSort_forEach = function(object, block, context) {
	for (var key in object) {
		if (typeof this.prototype[key] == "undefined") {
			block.call(context, object[key], key, object);
		}
	}
};

// character enumeration
String.gridSort_forEach = function(string, block, context) {
	Array.gridSort_forEach(string.split(""), function(chr, index) {
		block.call(context, chr, index, string);
	});
};

// globally resolve gridSort_forEach enumeration
var gridSort_forEach = function(object, block, context) {
	if (object) {
		var resolve = Object; // default
		if (object instanceof Function) {
			// functions have a "length" property
			resolve = Function;
		} else if (object.gridSort_forEach instanceof Function) {
			// the object implements a custom gridSort_forEach method so use that
			object.gridSort_forEach(block, context);
			return;
		} else if (typeof object == "string") {
			// the object is a string
			resolve = String;
		} else if (typeof object.length == "number") {
			// the object is array-like
			resolve = Array;
		}
		resolve.gridSort_forEach(object, block, context);
	}
}

function findParentID(element, appendToFront) {
//	if (element.hasProperties)
//	{
	var parent = element.parentNode;
	if (parent != null && parent.id != "")
		return appendToFront + "_" + parent.id;
	else
		return findParentID(parent, appendToFront);
//	}
//	else
//		return null;
};

