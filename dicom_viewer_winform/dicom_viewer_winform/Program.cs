using FellowOakDicom;
using FellowOakDicom.Imaging;

namespace dicom_viewer_winform
{
    internal static class Program
    {
        /// <summary>
        /// ���ø����̼� ������:
        /// fo-dicom �ھ� ���񽺿� WinForms �̹��� �Ŵ����� ����� ��
        /// ���� �����մϴ�.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // fo-dicom ���� �ʱ�ȭ �� WinForms ������ ���
            new DicomSetupBuilder()
                .RegisterServices(services => services
                    .AddFellowOakDicom()                  // DICOM �ھ�
                    .AddImageManager<WinFormsImageManager>() // WinForms�� �̹��� �Ŵ���
                )
                .Build();

            // Windows Forms ���� �ʱ�ȭ
            ApplicationConfiguration.Initialize();

            // ���� �� ����
            Application.Run(new Form1());
        }
    }
}