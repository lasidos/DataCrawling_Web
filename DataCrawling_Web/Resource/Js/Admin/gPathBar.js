$(function () {
	$('#gPathBar button').bind('click', function () {
		$(this).blur();
		var bHidden = ($('aside').width() == 0);

		if (bHidden) {
			$('aside').animate({ width: 217 });
			$('#gPathBar').animate({ marginLeft: 217 });
			$('section').animate({ marginLeft: 217 }, function () {
				$('#gPathBar button').removeClass('gFolded');

				if (typeof (grid) == 'object' && typeof (grid.refreshLayout) == 'function') {
					grid.refreshLayout();
				}
			});
		}
		else {
			$('aside').animate({ width: 0 });
			$('#gPathBar').animate({ marginLeft: 0 });
			$('section').animate({ marginLeft: 0 }, function () {
				$('#gPathBar button').addClass('gFolded');

				if (typeof (grid) == 'object' && typeof (grid.refreshLayout) == 'function') {
					grid.refreshLayout();
				}
			});
		}
	});
});