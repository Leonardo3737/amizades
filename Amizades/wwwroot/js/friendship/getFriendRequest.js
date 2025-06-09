let friendRequestPage = 1;

function loadMoreRequests() {
    $.get(`/api/Friendship/friend-request?page=${friendRequestPage}`, function (html) {
        friendRequestPage++;
        const $container = $('#friendsRequest-container');
        $container.append(html);

        if (html.includes('id="hide-load-more-REQUEST"')) {
            $('#load-more-REQUEST').hide();
        }
    });
}