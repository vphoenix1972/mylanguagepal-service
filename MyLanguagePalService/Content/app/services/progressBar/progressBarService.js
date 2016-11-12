function ProgressBarService(ngProgressFactory) {
    this._ngProgress = ngProgressFactory.createInstance();
}

ProgressBarService.prototype.start = function () {
    /// <summary>
    /// Starts the progress bar from the beginning.
    /// </summary>

    this._ngProgress.reset();
    this._ngProgress.start();
}

ProgressBarService.prototype.complete = function () {
    /// <summary>
    /// Jumps to 100% progress and fades away progressbar.
    /// </summary>

    this._ngProgress.complete();
}

ProgressBarService.prototype.reset = function () {
    /// <summary>
    /// Jumps to 100% progress and fades away progressbar.        
    /// </summary>

    this._ngProgress.reset();
}