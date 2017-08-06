$(function () {

    var transactionType = null;

    Date.prototype.toString = function () {
        var day = this.getDate() < 10 ? "0" + this.getDate() : this.getDate();
        var month = this.getMonth() < 9 ? "0" + (this.getMonth() + 1) : + this.getMonth() + 1;
        var year = this.getFullYear().toString().substr(2);

        var hour = this.getHours() < 10 ? "0" + this.getHours() : this.getHours();
        var minutes = this.getMinutes() < 10 ? "0" + this.getMinutes() : this.getMinutes();
        var seconds = this.getSeconds() < 10 ? "0" + this.getSeconds() : this.getSeconds();
        return [hour, minutes, seconds].join(":") + " " + [day, month, year].join(".");
    };

    transactionTemplate = $('.modal-content').html();

    transactionInProgressTemplate =
        '<div class="modal-body"> \
            Transaction in progress... \
        </div>';

    updateStocks = function (marketFeed, userFeed) {
        $('#connectionStatus').html('');
        updateStockPrices(marketFeed);
        updateUserWallet(userFeed);
    };

    updateStockPrices = function (feed) {
        $('#publicationDate').text(new Date(feed.PublicationDate).toString());
        $('#marketStock').html('');

        feed.StockList.forEach(function (stock) {
            stock.Price = stock.Price.toFixed(4);

            $('#marketStock').append(
                "<tr> \
                    <td>" + stock.Name + "</td> \
                    <td>" + stock.Price + "</td> \
                    <td><button \
                            data-action=\"Buy\" \
                            data-code=\"" + stock.Code + "\"  \
                            data-unit=\"" + stock.Unit + "\" \
                            data-price=\"" + stock.Price + "\" \
                            data-toggle=\"modal\" \
                            data-target=\"#transactionModal\" \
                    > Buy</button>"
            );

            var userPriceCell = $('#userStock tr[data-code="' + stock.Code + '"] td:nth-of-type(2)');
            var userAmountCell = $('#userStock tr[data-code="' + stock.Code + '"] td:nth-of-type(3)');
            var userValueCell = $('#userStock tr[data-code="' + stock.Code + '"] td:nth-of-type(4)');

            userPriceCell.text(stock.Price);
            userValueCell.text((stock.Price * userAmountCell.html()).toFixed(4));
        });
    };

    updateUserWallet = function (feed) {
        $('#userFounds').html(feed.Founds.toFixed(4));
        $('#userStock').html('');

        feed.StockList.forEach(function (stock) {
            stock.Price = stock.Price.toFixed(4);

            $('#userStock').append(
                "<tr> \
                    <td>" + stock.Name + "</td> \
                    <td>" + stock.Price + "</td> \
                    <td>" + stock.Amount + "</td> \
                    <td>" + (stock.Amount * stock.Price).toFixed(4) + "</td> \
                    <td><button \
                            data-action=\"Sell\" \
                            data-code=\"" + stock.Code + "\"  \
                            data-unit=\"" + stock.Unit + "\" \
                            data-price=\"" + stock.Price + "\" \
                            data-toggle=\"modal\" \
                            data-target=\"#transactionModal\" \
                    >Sell</button>"
            );
        });
    };

    prepareModalForTransaction = function (event) {
        var button = $(event.relatedTarget);
        var action = button.data('action');
        var code = button.data('code');
        var unit = button.data('unit');
        var price = button.data('price');

        var modal = $(this);
        modal.removeClass('alert alert-danger alert-success');
        modal.find('.modal-content').html(transactionTemplate);
        modal.find('.modal-body label').text(action + " " + code);
        modal.find('.modal-body input').attr('min', unit);
        modal.find('.modal-body input').attr('step', unit);
        $('#price').text(price);
        $('#amount').val(unit);
        $('#sum').text(price * unit);
        $('#transactionConfirmButton').data('action', action);
        $('#transactionConfirmButton').data('code', code);
        $('#transactionConfirmButton').on('click', executeTransaction);
        $('#amount').on('change', updateModalStockValue);
    };

    updateModalStockValue = function () {
        $('#sum').text($('#price').text() * $('#amount').val());
    };

    getErrorMessage = function (code) {
        errorMessages = [];
        errorMessages['Buy'] = [
            "Transaction Successful!",
            "Market doesn't have stock with selected code",
            "Sorry, we can't do that right now",
            "Market doesn't have required amount of stock",
            "You don't have enough money to buy that",
            "We have some problems, try again later"
        ];

        errorMessages['Sell'] = [
            "Transaction Successful!",
            "You don't have stock with selected code",
            "Sorry, we can't do that right now",
            "You don't have required amount of stock",
            "",
            "We have some problems, try again later"
        ];

        return errorMessages[transactionType][code];
    };

    executeTransaction = function (event) {
        button = $(event.target);
        transactionType = button.data('action');

        switch (button.data('action')) {
            case 'Sell':
                marketHub.server.sell($('#amount').val(), button.data('code'));
                break;
            case 'Buy':
                marketHub.server.buy($('#amount').val(), button.data('code'));
        }

        $('.modal-content').html(transactionInProgressTemplate);
    };

    displayTransactionResult = function (code) {
        if (code !== 0) {
            $('.modal').addClass('alert alert-danger');
        } else {
            $('.modal').addClass('alert alert-success');
        }

        $('.modal-body').text(getErrorMessage(code));
    };

    onTransactionComplete = function (result, userWallet) {
        displayTransactionResult(result);
        updateUserWallet(userWallet);
    };

    clearView = function () {
        $('#userFounds, #publicationDate').html(' -- ');
        $('#userStock, #marketStock').html('');
    }

    onConnectionComplete = function () {
        $('#connectionStatus').html('<div class="alert alert-info text-center"><div class="loading"><strong>Connection estabilished.</strong> Waiting for data</div></div>');
    }

    stocksUpdateErrorHandler = function () {
        clearView();
        $('#connectionStatus').html('<div class="alert alert-danger text-center"><div><strong>Unable to download stock prices.</strong> Try again later</div></div>');
    };

    $.connection.hub.reconnecting(function () {
        clearView();
        $('#connectionStatus').html('<div class="alert alert-warning text-center"><div class="loading"><strong>Connection Lost.</strong> Reconnecting</div></div>');
    });

    $.connection.hub.disconnected(function () {
        clearView();
        $('#connectionStatus').html('<div class="alert alert-danger text-center"><strong>Connection Lost.</strong> Unable to reconnect. Try again later.</div>');
    });

    var marketHub = $.connection.stockMarketHub;

    

    marketHub.client.updateStocks = updateStocks;
    marketHub.client.updateMarketStock = updateStockPrices;
    marketHub.client.completeTransaction = onTransactionComplete;
    marketHub.client.errorOnStocksUpdate = stocksUpdateErrorHandler;
    marketHub.client.onConnected = onConnectionComplete;
    
    $('#transactionModal').on('show.bs.modal', prepareModalForTransaction);

    $.connection.hub.start();
});