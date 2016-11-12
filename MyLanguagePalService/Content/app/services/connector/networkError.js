function NetworkError() {
    CustomError.call(this, 'NetworkError');
    this.name = 'NetworkError';
}

NetworkError.prototype = Object.create(CustomError.prototype);
NetworkError.prototype.constructor = NetworkError;