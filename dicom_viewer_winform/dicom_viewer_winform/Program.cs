using FellowOakDicom;
using FellowOakDicom.Imaging;

namespace dicom_viewer_winform
{
    internal static class Program
    {
        /// <summary>
            Application.Run(new DicomViewerForm());
        /// fo-dicom 코어 서비스와 WinForms 이미지 매니저를 등록한 후
        /// 폼을 실행합니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // fo-dicom 서비스 초기화 및 WinForms 렌더러 등록
            new DicomSetupBuilder()
                .RegisterServices(services => services
                    .AddFellowOakDicom()                  // DICOM 코어
                    .AddImageManager<WinFormsImageManager>() // WinForms용 이미지 매니저
                )
                .Build();

            // Windows Forms 구성 초기화
            ApplicationConfiguration.Initialize();

            // 메인 폼 실행
            Application.Run(new Form1());
        }
    }
}