namespace FxtDataAcquisition.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Project
    {
        public int ProjectId { get; set; }
        /// <summary>
        /// ¥������
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// ����ID
        /// </summary>
        public int? SubAreaId { get; set; }
        /// <summary>
        /// �ڵغ�
        /// </summary>
        public string FieldNo { get; set; }
        /// <summary>
        /// ����;
        /// </summary>
        public int PurposeCode { get; set; }
        /// <summary>
        /// ¥�̵�ַ
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// ռ�����
        /// </summary>
        public decimal? LandArea { get; set; }
        /// <summary>
        /// ������ʼ����
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// ����ʹ������
        /// </summary>
        public int? UsableYear { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        public decimal? BuildingArea { get; set; }
        /// <summary>
        /// ���������
        /// </summary>
        public decimal? SalableArea { get; set; }
        /// <summary>
        /// �ݻ���
        /// </summary>
        public decimal? CubageRate { get; set; }
        /// <summary>
        /// �̻���
        /// </summary>
        public decimal? GreenRate { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public DateTime? BuildingDate { get; set; }
        /// <summary>
        /// �ⶥ����
        /// </summary>
        public DateTime? CoverDate { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public DateTime? SaleDate { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        public DateTime? JoinDate { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// �ڲ��Ϲ�����
        /// </summary>
        public DateTime? InnerSaleDate { get; set; }
        /// <summary>
        /// ��Ȩ��ʽ
        /// </summary>
        public int? RightCode { get; set; }
        /// <summary>
        /// ��λ��
        /// </summary>
        public int? ParkingNumber { get; set; }
        /// <summary>
        /// ��Ŀ����
        /// </summary>
        public decimal? AveragePrice { get; set; }
        /// <summary>
        /// �����绰
        /// </summary>
        public string ManagerTel { get; set; }
        /// <summary>
        /// ��ҵ��
        /// </summary>
        public string ManagerPrice { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        public int? TotalNum { get; set; }
        /// <summary>
        /// �ܶ���
        /// </summary>
        public int? BuildingNum { get; set; }
        /// <summary>
        /// ��Ŀ�ſ�
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// ������������
        /// </summary>
        public int? BuildingTypeCode { get; set; }
        /// <summary>
        /// ������ʱ��
        /// </summary>
        public DateTime? UpdateDateTime { get; set; }
        /// <summary>
        /// �칫���
        /// </summary>
        public decimal? OfficeArea { get; set; }
        /// <summary>
        /// ������;���
        /// </summary>
        public decimal? OtherArea { get; set; }
        /// <summary>
        /// ���ع滮��;
        /// </summary>
        public string PlanPurpose { get; set; }
        /// <summary>
        /// �۸����ʱ��
        /// </summary>
        public DateTime? PriceDate { get; set; }
        /// <summary>
        /// �Ƿ���ɻ�������
        /// </summary>
        public int? IsComplete { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public string OtherName { get; set; }

        public DateTime? SaveDateTime { get; set; }

        public string SaveUser { get; set; }
        /// <summary>
        /// �۸�����ϵ��
        /// </summary>
        public decimal? Weight { get; set; }
        /// <summary>
        /// ��ҵ���
        /// </summary>
        public decimal? BusinessArea { get; set; }
        /// <summary>
        /// ��ҵ���
        /// </summary>
        public decimal? IndustryArea { get; set; }
        /// <summary>
        /// �Ƿ�ɹ�
        /// </summary>
        public int? IsEValue { get; set; }
        /// <summary>
        /// ƴ����д
        /// </summary>
        public string PinYin { get; set; }
        /// <summary>
        /// ����ID
        /// </summary>
        public int CityID { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        public int AreaID { get; set; }

        public string OldId { get; set; }

        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// ����ID
        /// </summary>
        public int? AreaLineId { get; set; }

        public int? Valid { get; set; }
        /// <summary>
        /// ���̾���
        /// </summary>
        public decimal? SalePrice { get; set; }

        public int? FxtCompanyId { get; set; }
        /// <summary>
        /// ¥������ȫƴ
        /// </summary>
        public string PinYinAll { get; set; }

        public decimal? X { get; set; }

        public decimal? Y { get; set; }

        public int? XYScale { get; set; }

        public string Creator { get; set; }
        /// <summary>
        /// �Ƿ��¥��
        /// </summary>
        public int? IsEmpty { get; set; }

        public int? TotalId { get; set; }
        /// <summary>
        /// ��
        /// </summary>
        public string East { get; set; }
        /// <summary>
        /// ��
        /// </summary>
        public string West { get; set; }
        /// <summary>
        /// ��
        /// </summary>
        public string South { get; set; }
        /// <summary>
        /// ��
        /// </summary>
        public string North { get; set; }

        public int? FxtProjectId { get; set; }
        /// <summary>
        /// ״̬
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// ͣ��״��
        /// </summary>
        public int? ParkingStatus { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public int? BuildingQuality { get; set; }
        /// <summary>
        /// С����ģ
        /// </summary>
        public int? HousingScale { get; set; }
        /// <summary>
        /// ¥����ע
        /// </summary>
        public string BuildingDetail { get; set; }
        /// <summary>
        /// ���ű�ע
        /// </summary>
        public string HouseDetail { get; set; }
        /// <summary>
        /// ��������;
        /// </summary>
        public string BasementPurpose { get; set; }
        /// <summary>
        /// ��ҵ��������
        /// </summary>
        public int? ManagerQuality { get; set; }
        /// <summary>
        /// �豸��ʩ
        /// </summary>
        public string Facilities { get; set; }
        /// <summary>
        /// ���׵ȼ�
        /// </summary>
        public int? AppendageClass { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        public string RegionalAnalysis { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public string Wrinkle { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public string Aversion { get; set; }
        /// <summary>
        /// ��λ����
        /// </summary>
        public string ParkingDesc { get; set; }


        public int? PhotoCount { get; set; }
        public int? TempletId { get; set; }
        
        
        public virtual ICollection<Building> Buildings { get; set; }

        //public virtual AllotFlow AllotFlow { get; set; }

        public virtual ICollection<PAppendage> Appendages { get; set; }
        public virtual ICollection<PCompany> Companys { get; set; }
        public virtual Templet Templet { get; set; }


    }
}
