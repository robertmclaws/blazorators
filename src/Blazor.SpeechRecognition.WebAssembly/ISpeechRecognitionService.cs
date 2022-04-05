﻿// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.JSInterop;

/// <summary>
/// A service the exposes various JavaScript interop capabilities specific to the
/// <c>speechRecognition</c> APIs. See <a href="https://developer.mozilla.org/docs/Web/API/SpeechRecognition"></a>
/// </summary>
public interface ISpeechRecognitionService : IAsyncDisposable
{
    /// <summary>
    /// Call once, before using in the consuming components
    /// <c>OnAfterRenderAsync(bool firstRender)</c> override, when firstRender is <c>true</c>.
    /// </summary>
    Task InitializeModuleAsync();

    /// <summary>
    /// Cancels the active speech recognition session.
    /// </summary>
    /// <param name="isAborted">
    /// Is aborted controls which API to call,
    /// either <c>speechRecognition.stop</c> or <c>speechRecognition.abort</c>.
    /// </param>
    void CancelSpeechRecognition(bool isAborted);

    /// <summary>
    /// Starts the speech recognition process. Callbacks will be invoked on
    /// the <paramref name="component"/> for the given method names.
    /// </summary>
    /// <typeparam name="TComponent">The consuming component (or object).</typeparam>
    /// <param name="component">The calling Razor (or Blazor) component.</param>
    /// <param name="language">The BCP47 language tag.</param>
    /// <param name="onResultCallbackMethodName">
    /// Expects the name of a <c>"JSInvokableAttribute"</c> C# method with the
    /// following <see cref="Action{String, SpeechRecognitionResult}"/> signature.
    /// </param>
    /// <param name="onErrorCallbackMethodName">
    /// Expects the name of a <c>"JSInvokableAttribute"</c> C# method with the
    /// following <see cref="Action{String, SpeechRecognitionErrorEvent}"/> signature.
    /// </param>
    /// <param name="onStartCallbackMethodName">
    /// Expects the name of a <c>"JSInvokableAttribute"</c> C# method with the
    /// following <see cref="Action{String}"/>.
    /// </param>
    /// <param name="onEndCallbackMethodName">
    /// Expects the name of a <c>"JSInvokableAttribute"</c> C# method with the
    /// following <see cref="Action{String}"/> signature.
    /// </param>
    void RecognizeSpeech<TComponent>(
        TComponent component,
        string language,
        string onResultCallbackMethodName,
        string? onErrorCallbackMethodName = null,
        string? onStartCallbackMethodName = null,
        string? onEndCallbackMethodName = null) where TComponent : class;

    /// <summary>
    /// Starts the speech recognition process. Returns an <see cref="IDisposable"/>
    /// that acts as the subscription. The various callbacks are invoked as they occur,
    /// and will continue to fire until the subscription is disposed of.
    /// </summary>
    /// <param name="language">The BCP47 language tag.</param>
    /// <param name="onRecognized">The callback to invoke when <c>onrecognized</c> fires.</param>
    /// <param name="onError">The optional callback to invoke when <c>onerror</c> fires.</param>
    /// <param name="onStarted">The optional callback to invoke when <c>onstarted</c> fires.</param>
    /// <param name="onEnded">The optional callback to invoke when <c>onended</c> fires.</param>
    /// <returns>
    /// To unsubscribe from the speech recognition, call
    /// <see cref="IDisposable.Dispose"/>.
    /// </returns>
    IDisposable RecognizeSpeech(
        string language,
        Action<string> onRecognized,
        Action<SpeechRecognitionErrorEvent>? onError = null,
        Action? onStarted = null,
        Action? onEnded = null);
}
