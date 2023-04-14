using System.Collections.Generic;
using System.Linq;

namespace Monolith.diagnostics;

public static class TimeHelper
{
	public static long TotalFrames { get; private set; }
	public static float TotalSeconds { get; private set; }
	public static float AverageFramesPerSecond { get; private set; }
	public static float CurrentFramesPerSecond { get; private set; }

	public const int maximumSamples = 100;

	private static readonly Queue<float> sampleBuffer = new ();

	public static void Update(float deltaTime)
	{
		CurrentFramesPerSecond = 1.0f / deltaTime;

		sampleBuffer.Enqueue(CurrentFramesPerSecond);

		if (sampleBuffer.Count > maximumSamples) {
			sampleBuffer.Dequeue();
			AverageFramesPerSecond = sampleBuffer.Average(i => i);
		}
		else {
			AverageFramesPerSecond = CurrentFramesPerSecond;
		}

		TotalFrames++;
		TotalSeconds += deltaTime;
	}
}