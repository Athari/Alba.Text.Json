<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:param name="remove-prefixes" select="''" />

  <xsl:strip-space elements="*"/>
  <xsl:preserve-space elements="doc member" />

  <xsl:template name="identity" match="node() | @*">
    <xsl:copy>
      <xsl:apply-templates select="node() | @*" />
    </xsl:copy>
  </xsl:template>

  <xsl:template match="member">
    <xsl:variable name="remove">
      <xsl:call-template name="check-prefixes-recurse">
        <xsl:with-param name="name" select="@name" />
        <xsl:with-param name="prefixes" select="$remove-prefixes" />
      </xsl:call-template>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="$remove = 'true'" />
      <xsl:otherwise>
        <xsl:call-template name="identity" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="check-prefixes-recurse">
    <xsl:param name="name" />
    <xsl:param name="prefixes" />
    <xsl:variable name="prefix" select="substring-before(concat($prefixes, ';'), ';')" />
    <xsl:variable name="remaining" select="substring-after($prefixes, ';')" />
    <xsl:choose>
      <xsl:when test="$prefix != '' and contains($name, $prefix)">
        <xsl:value-of select="'true'" />
      </xsl:when>
      <xsl:when test="$remaining != ''">
        <xsl:call-template name="check-prefixes-recurse">
          <xsl:with-param name="name" select="$name" />
          <xsl:with-param name="prefixes" select="$remaining" />
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="'false'" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

</xsl:stylesheet>