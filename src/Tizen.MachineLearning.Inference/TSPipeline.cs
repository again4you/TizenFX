using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace Tizen.MachineLearning.Inference
{
    public static class TSPipeline
    {
        public static bool Pipeline_Success00()
        {
            string desc = "videotestsrc num_buffers=2 ! fakesink";
            Pipeline p = new Pipeline(desc);

            p.Dispose();
            return true;
        }

        public static bool Pipeline_Success01()
        {
            string desc = "videotestsrc num_buffers=2 ! videoconvert ! videoscale ! video/x-raw,format=RGBx,width=224,height=224 ! tensor_converter ! fakesink";
            Pipeline p = new Pipeline(desc);

            p.Dispose();
            return true;
        }

        public static bool Pipeline_Success02()
        {
            string desc = "videotestsrc is-live=true ! videoconvert ! videoscale ! video/x-raw,format=RGBx,width=224,height=224,framerate=60/1 ! tensor_converter ! valve name=valvex ! valve name=valvey ! input-selector name=is01 ! tensor_sink name=sinkx";
            Pipeline p = new Pipeline(desc);

            p.Start();

            Thread.Sleep(50000);
            PipelineState state = p.GetState();

            p.Stop();
            p.Dispose();

            return true;
        }
    }
}
